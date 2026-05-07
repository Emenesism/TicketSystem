using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Api.Dtos.Admins;

public class UpdateAdminDto
{
    [MinLength(3, ErrorMessage = "Name Must At Least 3 Char")]
    [MaxLength(50, ErrorMessage = "Name Is Too Long")]
    public string Name { get; set; } = string.Empty;

    [MinLength(8, ErrorMessage = "Password Must At Least 8 Char")]
    public string Password { get; set; } = string.Empty;
}
