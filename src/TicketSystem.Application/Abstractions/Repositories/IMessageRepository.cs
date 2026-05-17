using TicketSystem.Domain.Entities;

namespace TicketSystem.Application.Abstractions.Repositories;

public interface ITicketMessageRepository
{
    Task CreateTicketMessage(TicketMessage message);

    Task<List<TicketMessage>> GetAllMessageRelatedOfTicket(Guid ticketId);

    Task<TicketMessage?> GetMessageById(Guid messageId);

    Task CreateAttachment(Attachment attachment);

    Task<Attachment?> GetAttachmentById(Guid attachmentId);

    Task DeleteAttachment(Attachment attachment);

    Task<bool> DeleteMessage(Guid messageId, Guid senderId);
}
