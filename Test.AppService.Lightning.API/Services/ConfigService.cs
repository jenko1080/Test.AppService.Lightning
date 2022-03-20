using Microsoft.Spatial;
using Test.AppService.Lightning.API.Models;
using Test.AppService.Lightning.API.Services.Interfaces;

namespace Test.AppService.Lightning.API.Services
{
    public class ConfigService : IConfigService
    {
        private readonly IConfiguration _configuration;

        public ConfigService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public BoundingBox GetBoundingBox()
        {
            double latMin, latMax, lonMin, lonMax;

            latMin = double.TryParse(_configuration["BoundingBox:latMin"], out latMin) ? latMin : -39.25;
            latMax = double.TryParse(_configuration["BoundingBox:latMax"], out latMax) ? latMax : -33.75;
            lonMin = double.TryParse(_configuration["BoundingBox:lonMin"], out lonMin) ? lonMin : 140.0;
            lonMax = double.TryParse(_configuration["BoundingBox:lonMax"], out lonMax) ? lonMax : 150.0;

            var bottomLeft = GeographyPoint.Create(latMin, lonMin);
            var topRight = GeographyPoint.Create(latMax, lonMax);

            return new BoundingBox
            {
                BottomLeft = bottomLeft,
                TopRight = topRight,
            };
        }
    }
}
