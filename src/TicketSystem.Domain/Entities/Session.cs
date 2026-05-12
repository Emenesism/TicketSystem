namespace TicketSystem.Domain.Entities;

public class Session(string tokenHash, Guid? adminId, Guid? userId, bool isAdmin, DateTime expiresAt)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TokenHash { get; set; } = tokenHash;
    public Guid? AdminId { get; set; } = adminId;
    public Guid? UserId { get; set; } = userId;
    public bool IsAdmin { get; set; } = isAdmin;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; } = expiresAt;
}
