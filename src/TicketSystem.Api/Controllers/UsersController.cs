using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Application.Abstractions.Repositories;
using TicketSystem.Domain.Entities;
using TicketSystem.Api.Dtos.Users;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TicketSystem.Api.Controllers;


[ApiController]
[Route("users")]
[Authorize(Roles = "Admin")]
public sealed class UserController(IUserRepository userRepository) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<List<GetUserResponse>>> GetAllUsers()
    {
        var usersFromDb = await userRepository.GetUsersAsync();

        var result = usersFromDb.Select(s => new GetUserResponse
        {
            Name = s.Name,
            Username = s.Username,
            CreatedAt = s.CreatedAt
        });

        return Ok(result);
    }

    [HttpGet("by-username")]
    public async Task<ActionResult<GetUserResponse>> GetUserViaUsername([FromQuery] string username)
    {
        var userFromDb = await userRepository.GetUserByUsername(username);

        if (userFromDb is null)
        {
            return NotFound(new
            {
                message = $"User With Username {username} Not Found"
            });
        }

        return Ok(new GetUserResponse
        {
            Name = userFromDb.Name,
            Username = userFromDb.Username,
            CreatedAt = userFromDb.CreatedAt

        });
    }

    [HttpGet("by-name")]
    public async Task<ActionResult<GetUserResponse>> GetUserViaName([FromQuery] string name)
    {
        var userFromDb = await userRepository.GetUserByName(name);

        if (userFromDb is null)
        {
            return NotFound(new
            {
                message = $"User With Name {name} Not Found"
            });
        }

        return Ok(new GetUserResponse
        {
            Name = userFromDb.Name,
            Username = userFromDb.Username,
            CreatedAt = userFromDb.CreatedAt
        });
    }

    [HttpGet("before-date")]
    public async Task<ActionResult<List<GetUserResponse>>> GetUserBeforeCertainDate([FromQuery] DateTime date)
    {

        var utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);

        var usersFromDb = await userRepository.GetAllUserBeforeCertianDate(utcDate);

        if (usersFromDb is null)
        {
            return NotFound(new
            {
                Message = $"No User Exist Before date {date}"
            });
        }

        var result = usersFromDb.Select(s => new GetUserResponse
        {
            Name = s.Name,
            Username = s.Username,
            CreatedAt = s.CreatedAt
        }).ToList();

        return Ok(result);
    }

    [HttpGet("after-date")]
    public async Task<ActionResult<List<GetUserResponse>>> GetUserAfterCertainDate([FromQuery] DateTime date)
    {
        var utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        var usersFromDb = await userRepository.GetAllUserAfterCertianDate(utcDate);

        if (usersFromDb is null)
        {
            return NotFound(new
            {
                Message = $"No User Exist After date {date}"
            });
        }

        var result = usersFromDb.Select(s => new GetUserResponse
        {
            Name = s.Name,
            Username = s.Username,
            CreatedAt = s.CreatedAt
        }).ToList();

        return Ok(result);
    }

}
