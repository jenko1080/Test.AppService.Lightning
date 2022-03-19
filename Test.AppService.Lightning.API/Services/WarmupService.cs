using Test.AppService.Lightning.API.Enums;
using Test.AppService.Lightning.API.Models;
using Test.AppService.Lightning.API.Services.Interfaces;

namespace Test.AppService.Lightning.API.Services
{
    public class WarmupService : IWarmupService
    {
        private readonly ILogger<WarmupService> _logger;
        private readonly ITcpWorkerService _tcpWorkerService;
        private readonly ITablesService _tablesService;

        public WarmupService(ILogger<WarmupService> logger, ITcpWorkerService tcpWorkerService, ITablesService tablesService)
        {
            _logger = logger;
            _tcpWorkerService = tcpWorkerService;
            _tablesService = tablesService;
        }

        public async Task<bool> WarmupAsync()
        {
            // Check warmup state (skip everything else)
            while(!_tcpWorkerService.IsWarm())
            {
                _ = await _tablesService.AddToTable(CreateWarmupEntry(false));
                await Task.Delay(1000);
            }

            // Create entry
            _ = await _tablesService.AddToTable(CreateWarmupEntry(true));

            // Return status
            return true;
        }

        private WarmupEntry CreateWarmupEntry(bool isConnected)
        {
            return new WarmupEntry
            {
                DateTimeUtc = DateTime.UtcNow,
                Status = isConnected ? WarmupStatus.Ready : WarmupStatus.NotReady
            };
        }
    }
}
