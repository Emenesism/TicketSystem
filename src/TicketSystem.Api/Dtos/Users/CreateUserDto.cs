using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Api.Dtos.Users;

public class CreateUserDto
{
    [Required(ErrorMessage = "Name Property Is Required")]
    [MinLength(3, ErrorMessage = "Name Must At Least 3 Char")]
    [MaxLength(50, ErrorMessage = "Name Is Too Long")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Username Property Is Required")]
    [MinLength(3, ErrorMessage = "Username Must At Least 3 Char")]
    [MaxLength(50, ErrorMessage = "Username Is Too Long")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password Property Is Required")]
    [MinLength(8, ErrorMessage = "Password Must At Least 8 Char")]
    public string Password { get; set; } = string.Empty;
}
