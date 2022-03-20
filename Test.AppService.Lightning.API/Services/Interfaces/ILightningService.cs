using Test.AppService.Lightning.API.Models;

namespace Test.AppService.Lightning.API.Services.Interfaces
{
    public interface ILightningService
    {
        Task HandleLightningJson(string lightning);

        bool IsConnectionMessage(string lightning);
        bool IsInBoundingBox(LightningStrokeEntry l);
    }
}