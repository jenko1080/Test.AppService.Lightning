using Test.AppService.Lightning.API.Services.Interfaces;

namespace Test.AppService.Lightning.API.Services
{
    public class TcpBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TcpBackgroundService> _logger;

        public TcpBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<TcpBackgroundService> logger
        )
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(TcpBackgroundService)} starting.");
            await DoWorkAsync(stoppingToken);
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var worker = scope.ServiceProvider.GetRequiredService<ITcpWorkerService>();
                await worker.ListenTcp(stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(TcpBackgroundService)} stopping.");
            await base.StopAsync(stoppingToken);
        }
    }
}
