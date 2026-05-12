using TicketSystem.Api.Dtos;
using TicketSystem.Domain.Entities;
using TicketSystem.Application.Abstractions.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TicketSystem.Api.Dtos.Messages;
using System.Security.Claims;
using TicketSystem.Api.Dtos.Tickets;


[ApiController]
[Route("message")]
[Authorize]
public class MessageController(ITicketMessageRepository messageRepository) : ControllerBase
{

    [HttpPost]
    public async Task<ActionResult<MessageResponse>> CreateMessage([FromBody] CreateMessageDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var senderId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var message = new TicketMessage(dto.Content, senderId, dto.TicketId);

        await messageRepository.CreateTicketMessage(message);

        return Ok(new MessageResponse
        {
            Id = message.Id,
            SenderId = message.SenderId,
            TicketId = message.TicketId,
            Content = message.Content,
            CreatedAt = message.CreatedAt
        });
    }

    [HttpPost("all")]
    public async Task<ActionResult<List<MessageResponse>>> GetAllMessageRelatedToTicket([FromBody] TicketIdDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var tickets = await messageRepository.GetAllMessageRelatedOfTicket(dto.TicketId);

        var result = tickets.Select(s => new MessageResponse
        {
            Content = s.Content,
            Id = s.Id,
            SenderId = s.SenderId,
            CreatedAt = s.CreatedAt,
            TicketId = s.TicketId
        }).ToList();

        return Ok(result);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteMessage([FromBody] MessageIdDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem();
        }

        var senderId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var status = await messageRepository.DeleteMessage(dto.MessageId, senderId);

        if (!status)
        {
            return NotFound(new
            {
                message = "The Message Is Not Found Or You Delete Another Person Message"
            });
        }

        return Ok(new
        {
            message = "Done"
        });

    }
}
