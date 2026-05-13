namespace TicketSystem.Domain.Entities;

public class Session(
    string tokenHash,
    Guid? adminId,
    Guid? userId,
    bool isAdmin,
    DateTime expiresAt,
    string? userAgent,
    string? ipAddress)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string TokenHash { get; set; } = tokenHash;
    public Guid? AdminId { get; set; } = adminId;
    public Guid? UserId { get; set; } = userId;
    public bool IsAdmin { get; set; } = isAdmin;
    public string? UserAgent { get; set; } = userAgent;
    public string? IpAddress { get; set; } = ipAddress;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; } = expiresAt;
    public DateTime? RevokeAt { get; set; }
    public DateTime LastUsageAt { get; set; } = DateTime.UtcNow;
}
