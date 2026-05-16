namespace TicketSystem.Domain.Entities;

using System.Text.Json;

public class Admin(string name, string username, string passwordHash, bool isSuperAdmin)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public string Username { get; set; } = username;
    public bool IsDeleted { get; set; } = false;
    public string PasswordHash { get; set; } = passwordHash;
    public string Roles { get; set; } = "[]";
    public bool IsSuperAdmin { get; set; } = isSuperAdmin;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Ticket> Tickets { get; set; } = [];

    public List<string> GetRoles()
    {
        return JsonSerializer.Deserialize<List<string>>(Roles) ?? [];
    }

    public void StoreRoles(List<string> roles)
    {
        Roles = JsonSerializer.Serialize(roles);
    }

}
