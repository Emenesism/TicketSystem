namespace TicketSystem.Api.Dtos.Users;

public sealed class GetUserResponse
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
