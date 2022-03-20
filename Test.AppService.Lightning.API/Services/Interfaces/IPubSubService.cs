using Test.AppService.Lightning.API.Models;

namespace Test.AppService.Lightning.API.Services.Interfaces
{
    public interface IPubSubService
    {
        Task<string> GetPubSubClientUriAsync();
        Task PublishKeepAliveMessageAsync(KeepAliveMessage keepAlive);
        Task PublishLightningMessageAsync(LightningStrokeEntry lightning);
    }
}