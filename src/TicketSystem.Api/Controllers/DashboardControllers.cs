
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Application.Abstractions.Repositories;
using TicketSystem.Application.Dtos.Dashboard;

namespace TicketSystem.Api.Controllers;


[ApiController]
[Route("dashboard")]
[Authorize(Roles = "SuperAdmin")]
public class DashboardController(ISessionRepo sessionRepo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AllSessionResponse>>> GetAllSession()
    {
        var session = await sessionRepo.GetActiveSessionForDashboard();

        return Ok(session);
    }
}
