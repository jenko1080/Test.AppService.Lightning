using Test.AppService.Lightning.API.Models;

namespace Test.AppService.Lightning.API.Services.Interfaces
{
    public interface IConfigService
    {
        BoundingBox GetBoundingBox();
    }
}