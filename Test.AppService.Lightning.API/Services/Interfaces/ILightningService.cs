namespace Test.AppService.Lightning.API.Services.Interfaces
{
    public interface ILightningService
    {
        Task HandleLightningJson(string lightning);
    }
}