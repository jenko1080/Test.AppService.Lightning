using Microsoft.AspNetCore.Mvc;
using Test.AppService.Lightning.API.Models;
using Test.AppService.Lightning.API.Services.Interfaces;

namespace Test.AppService.Lightning.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PubSubController : ControllerBase
    {
        private readonly ILogger<PubSubController> _logger;
        private readonly IPubSubService _pubSubService;

        public PubSubController(ILogger<PubSubController> logger, IPubSubService pubSubService)
        {
            _logger = logger;
            _pubSubService = pubSubService;
        }

        [HttpGet("negotiate")]
        public IActionResult Negotiate()
        {
            var response = new
            {
                url = _pubSubService.GetPubSubClientUriAsync()
            };

            return Ok(response);
        }

        [HttpGet("test")]
        public IActionResult TestStroke()
        {
            Random rnd = new Random();
            var latDrift = rnd.Next(-10000, 10000) / 10000.0f;
            var lonDrift = rnd.Next(-10000, 10000) / 10000.0f;

            var lightning = new LightningStrokeEntry
            {
                DateTimeUtc = DateTime.UtcNow,
                Latitude = -37.808820f + latDrift,
                Longitude = 144.973906f + lonDrift,
                Amplitude = 24.606f,
                Type = LightningStrokeType.CG,
                Height = 0,
                NumSensors = 5,
                NumPulses = 1
            };

            _pubSubService.PublishLightningMessageAsync(lightning);

            return Ok(lightning);
        }
    }
}
