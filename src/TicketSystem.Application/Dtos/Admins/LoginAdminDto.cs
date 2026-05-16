using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Application.Dtos.Admins;

public class LoginAdminDto
{
    [Required(ErrorMessage = "Username Property Is Required")]
    [MinLength(3, ErrorMessage = "Username Must At Least 3 Char")]
    [MaxLength(50, ErrorMessage = "Username Is Too Long")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password Property Is Required")]
    [MinLength(8, ErrorMessage = "Password Must At Least 8 Char")]
    public string Password { get; set; } = string.Empty;

}
