using Microsoft.AspNetCore.Mvc;

namespace TicketSystem.Api.Controllers;

[ApiController]
[Route("healthz")]
public sealed class HealthzController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "OK",
            answerTime = DateTime.UtcNow
        });
    }
}
