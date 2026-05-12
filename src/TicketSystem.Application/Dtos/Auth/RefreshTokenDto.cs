using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Application.Dtos.Auth;


public class RefreshTokenDto
{
    [Required(ErrorMessage = "Refresh Token Is Required.")]
    public string RefreshToken { get; set; } = string.Empty;
}
