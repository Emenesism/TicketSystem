namespace TicketSystem.Domain.Entities;


public class TicketMessage(string content, Guid senderId, Guid ticketId)
{
    public Guid Id { get; set; }

    public string Content { get; set; } = content;

    public Guid TicketId { get; set; } = ticketId;

    public Guid SenderId { get; set; } = senderId;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Ticket Ticket { get; set; } = null!;
}
