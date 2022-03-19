using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using Test.AppService.Lightning.API.Models;
using Test.AppService.Lightning.API.Services.Interfaces;

namespace Test.AppService.Lightning.API.Services
{
    // Create Background Service to listen to TCP messages
    public class TcpWorkerService : ITcpWorkerService
    {
        private const int _tcpLineCountMs = 600000; // 10 minutes

        private bool IsConnected = false;
        private bool _shouldContinue = false;
        private readonly ILogger<TcpWorkerService> _logger;
        private readonly ITablesService _tablesService;
        private readonly ILightningService _lightningService;
        private readonly IConfiguration _configuration;
        private readonly Stopwatch _stopWatch;

        public TcpWorkerService(ILogger<TcpWorkerService> logger, ITablesService tablesService, ILightningService lightningService, IConfiguration configuration)
        {
            _logger = logger;
            _tablesService = tablesService;
            _lightningService = lightningService;
            _configuration = configuration;

            _stopWatch = new Stopwatch();

            bool.TryParse(_configuration["Experiments:ShouldContinue"], out _shouldContinue);
        }

        public async Task ListenTcp(CancellationToken stoppingToken)
        {
            var lightningUri = _configuration["Lightning:Uri"];
            var lightningPort = int.Parse(_configuration["Lightning:Port"]);
            var lightningAuthString = _configuration["Lightning:AuthString"];

            if(_shouldContinue == false)
            {
                _logger.LogWarning("Holding lightning feed startup until _shouldContinue var is set to true... probably use `/warmup/continue` endpoint.");
                while (_shouldContinue == false)
                {
                    await Task.Delay(1000);
                }
            }

            _logger.LogInformation($"Connecting to Lightning Feed on TCP: {lightningUri}:{lightningPort}");

            // Create a TCP client
            using var client = new TcpClient();
            await client.ConnectAsync(lightningUri, lightningPort);

            // Create a stream for the TCP connection
            using var networkStream = client.GetStream();

            _logger.LogInformation($"Connected to Lightning Feed, attempting to authorise");

            // Write auth string to TCP stream (with a small pre-delay)
            await Task.Delay(10, stoppingToken);
            using var writer = new StreamWriter(networkStream);
            byte[] bytes = Encoding.UTF8.GetBytes($"{lightningAuthString}\r");
            await networkStream.WriteAsync(bytes, 0, bytes.Length);

            // Log new connection
            if (networkStream.Socket.Connected)
            {
                // Start Listening
                using var reader = new StreamReader(networkStream, Encoding.UTF8);

                var line = reader.ReadLine();

                // Confirm with first recieved packed that we're good to go
                if (!string.IsNullOrWhiteSpace(line) && _lightningService.IsConnectionMessage(line))
                {
                    // Log the success
                    await AddConnectionLog(ConnectionUpdateStatus.Connected, "Connected and authorised.");
                    IsConnected = true;

                    _stopWatch.Start();
                    var lineCount = 0;
                    while (!stoppingToken.IsCancellationRequested && networkStream.Socket.Connected)
                    {
                        // Read each new TCP line
                        line = reader.ReadLine();
                        lineCount++;
                        _logger.LogDebug($"TCP Line Recieved: {line}");

                        // Throw incoming data to LightningService to deal with
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            // Note: Do not await, drops rate from ~400 to ~22 during testing
                            _ = _lightningService.HandleLightningJson(line);
                        }

                        if (_stopWatch.ElapsedMilliseconds > _tcpLineCountMs)
                        {
                            _stopWatch.Stop();
                            _logger.LogInformation($"TCP Line Count in the last {_stopWatch.ElapsedMilliseconds} ms: {lineCount}.");
                            _ = AddConnectionLogUpdate(lineCount, _stopWatch.ElapsedMilliseconds);
                            _stopWatch.Restart();
                            lineCount = 0;
                        }
                    }
                }
                else
                {
                    await AddConnectionLog(ConnectionUpdateStatus.Failed, "Failed to authorise.");
                }
            }
            else
            {
                // Log the failure
                await AddConnectionLog(ConnectionUpdateStatus.Failed, "Failed to connect.");
            }

            // Cancellation requested, clean up
            if (stoppingToken.IsCancellationRequested)
            {
                _logger.LogWarning("Cancellation requested, closing connections");
                networkStream.Close();
                await AddConnectionLog(ConnectionUpdateStatus.Disconnected, "Cancellation requested.");

            }
            else if (!networkStream.Socket.Connected)
            {
                _logger.LogWarning("Unexpected socket close, cleaning up.");
                await AddConnectionLog(ConnectionUpdateStatus.Disconnected, "Unexpected socket disconnection.");
            }

            IsConnected = false;

            _logger.LogWarning("TCP Worker completed.");
        }

        public bool IsWarm()
        {
            return IsConnected;
        }

        // Connect Message
        private async Task AddConnectionLog(ConnectionUpdateStatus status, string message)
        {
            var connectionLogEntry = new ConnectionLogEntry
            {
                DateTimeUtc = DateTime.UtcNow,
                NewStatus = status,
                UpdateMessage = message
            };

            _ = await _tablesService.AddToTable(connectionLogEntry);
        }

        // Metrics
        private async Task AddConnectionLogUpdate(int numLines, long time)
        {
            var connectionLogEntry = new ConnectionLogEntry
            {
                DateTimeUtc = DateTime.UtcNow,
                NewStatus = ConnectionUpdateStatus.Update,
                UpdateMessage = $"Number of TCP lines parsed in the last {time} ms",
                Number = numLines
            };

            _ = await _tablesService.AddToTable(connectionLogEntry);
        }

        /// <summary>
        /// Allows the startup to continue, simple flag to allow testing of App Service warm up
        /// </summary>
        /// <returns></returns>
        public bool AllowContinue()
        {
            if(_shouldContinue == false)
            {
                _shouldContinue = true;
                return true;
            }
            return false;
        }
    }
}