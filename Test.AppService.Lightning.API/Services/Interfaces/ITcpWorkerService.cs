namespace Test.AppService.Lightning.API.Services.Interfaces
{
    public interface ITcpWorkerService
    {
        Task ListenTcp(CancellationToken stoppingToken);
        bool IsWarm();
        bool AllowContinue();
    }
}