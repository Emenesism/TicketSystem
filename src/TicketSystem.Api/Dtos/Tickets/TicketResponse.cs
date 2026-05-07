namespace TicketSystem.Api.Dtos.Tickets;


public class TicketResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsSolved { get; set; }
}
