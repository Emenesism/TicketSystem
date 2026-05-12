namespace TicketSystem.Domain.Entities;


public class User(string name, string username, string passwordHash)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public string Username { get; set; } = username;
    public bool IsDeleted { get; set; } = false;
    public string PasswordHash { get; set; } = passwordHash;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Ticket> Tickets { get; set; } = [];
}
