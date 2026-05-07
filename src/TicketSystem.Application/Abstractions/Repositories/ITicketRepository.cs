using TicketSystem.Domain.Entities;

namespace TicketSystem.Application.Abstractions.Repositories;

public interface ITicketRepository
{
    Task CreateTicket(Ticket ticket);

    Task<Ticket?> GetTicketViaTicketId(Guid ticketId);

    Task<List<Ticket>> GetAllUserTicket(Guid userId);

    Task<List<Ticket>> GetAllAdminTicket(Guid adminId);

    Task<List<Ticket>> GetAllNotAssingedTicket();

    Task<bool> AssignTicketToAdmin(Guid ticketId, Guid adminId);

    Task<bool> ChangeStatusOfSolveToTrue(Guid ticketId, Guid adminId);
}
