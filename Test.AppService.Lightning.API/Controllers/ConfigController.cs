using Microsoft.AspNetCore.Mvc;
using Test.AppService.Lightning.API.Models;
using Test.AppService.Lightning.API.Services.Interfaces;

namespace Test.AppService.Lightning.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;
        private readonly IConfigService _configService;
        private readonly ILightningService _lightningService;

        public ConfigController(ILogger<ConfigController> logger, IConfigService configService, ILightningService lightningService)
        {
            _logger = logger;
            _configService = configService;
            _lightningService = lightningService;
        }

        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("boundingbox")]
        public IActionResult GetBoundingBox()
        {
            return Ok(_configService.GetBoundingBox());
        }

        [HttpGet("boundingbox/testvic")]
        public IActionResult TestBoundingBox()
        {
            var lightning = new LightningStrokeEntry
            {
                DateTimeUtc = DateTime.UtcNow,
                Latitude = -37.808820f,
                Longitude = 144.973906f,
                Amplitude = 24.606f,
                Type = LightningStrokeType.CG,
                Height = 0,
                NumSensors = 5,
                NumPulses = 1
            };

            return Ok(_lightningService.IsInBoundingBox(lightning));
        }
    }
}
