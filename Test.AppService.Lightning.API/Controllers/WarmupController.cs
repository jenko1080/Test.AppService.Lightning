using Microsoft.AspNetCore.Mvc;
using System.Net;
using Test.AppService.Lightning.API.Services.Interfaces;

namespace Test.AppService.Lightning.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WarmupController : ControllerBase
    {
        private readonly ILogger<WarmupController> _logger;
        private readonly IWarmupService _warmupService;
        private readonly ITcpWorkerService _tcpWorkerService;

        public WarmupController(ILogger<WarmupController> logger, IWarmupService warmupService, ITcpWorkerService tcpWorkerService)
        {
            _logger = logger;
            _warmupService = warmupService;
            _tcpWorkerService = tcpWorkerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            // Start the warmup process
            await _warmupService.WarmupAsync();

            // Ok when the system is warmed up... there is no other option.
            return Ok();
        }

        [HttpGet("continue")]
        public IActionResult Continue()
        {
            return Ok(_tcpWorkerService.AllowContinue());
        }
    }
}
