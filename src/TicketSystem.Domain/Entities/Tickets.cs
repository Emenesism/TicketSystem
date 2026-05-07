

namespace TicketSystem.Domain.Entities;

public class Ticket(string title, Guid userId)
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = title;

    public bool Solved { get; set; } = false;
    public Guid UserId { get; set; } = userId;
    public User User { get; set; } = null!;

    public Guid? AdminId { get; set; }
    public Admin? Admin { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<TicketMessage> Messages { get; set; } = [];

}
