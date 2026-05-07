namespace TicketSystem.Api.Dtos.Messages;

public class MessageResponse
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid TicketId { get; set; }
    public Guid SenderId { get; set; }
    public DateTime CreatedAt { get; set; }

}
