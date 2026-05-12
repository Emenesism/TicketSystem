namespace TicketSystem.Application.Dtos.Users;

public class UpdateUserResponse
{
    public string Name { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
