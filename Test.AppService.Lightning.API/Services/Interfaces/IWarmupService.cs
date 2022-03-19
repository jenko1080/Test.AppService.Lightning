namespace Test.AppService.Lightning.API.Services.Interfaces
{
    public interface IWarmupService
    {
        Task<bool> WarmupAsync();
    }
}