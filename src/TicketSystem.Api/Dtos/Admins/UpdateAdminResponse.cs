namespace TicketSystem.Api.Dtos.Admins;

public class UpdateAdminResponse
{
    public string Name { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
