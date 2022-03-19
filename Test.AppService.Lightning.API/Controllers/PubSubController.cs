using Azure.Messaging.WebPubSub;
using Microsoft.AspNetCore.Mvc;
using Test.AppService.Lightning.API.Services.Interfaces;

namespace Test.AppService.Lightning.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PubSubController : ControllerBase
    {
        private readonly ILogger<PubSubController> _logger;
        private readonly IPubSubService _pubSubService;
        private readonly WebPubSubServiceClient _webPubSubServiceClient;

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
    }
}
