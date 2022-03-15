using Microsoft.AspNetCore.Mvc;

namespace Test.AppService.Lightning.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController : ControllerBase
{
    private readonly ILogger<StatusController> _logger;

    public StatusController(ILogger<StatusController> logger)
    {
        _logger = logger;
    }

    public IActionResult Get()
    {
        return Ok();
    }
}
