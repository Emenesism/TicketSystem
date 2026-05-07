using TicketSystem.Domain.Entities;

namespace TicketSystem.Application.Abstractions.Repositories;

public interface ITicketMessageRepository
{
    Task CreateTicketMessage(TicketMessage message);

    Task<List<TicketMessage>> GetAllMessageRelatedOfTicket(Guid ticketId);

    Task<bool> DeleteMessage(Guid messageId, Guid senderId);
}
