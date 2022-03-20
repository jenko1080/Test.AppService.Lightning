using Microsoft.AspNetCore.Mvc;
using Test.AppService.Lightning.API.Services.Interfaces;

namespace Test.AppService.Lightning.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;
        private readonly IConfigService _configService;

        public ConfigController(ILogger<ConfigController> logger, IConfigService configService)
        {
            _logger = logger;
            _configService = configService;
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
    }
}
