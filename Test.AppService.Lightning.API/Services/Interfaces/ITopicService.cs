using Test.AppService.Lightning.API.Models;

namespace Test.AppService.Lightning.API.Services.Interfaces
{
    public interface ITopicService
    {
        Task AddLightningMessage(LightningStrokeEntry stroke);
    }
}