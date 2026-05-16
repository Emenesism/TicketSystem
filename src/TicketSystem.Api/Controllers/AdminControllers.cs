using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Application.Abstractions.Repositories;
using TicketSystem.Domain.Entities;

namespace TicketSystem.Api.Controllers;

[ApiController]
[Route("admin")]
[Authorize(Roles = "SuperAdmin")]
public class AdminController(IAdminRepository adminRepo) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<List<Admin>>> GetAllAdmin()
    {
        var admins = await adminRepo.GetAllAdmins();

        return Ok(admins);
    }
}
