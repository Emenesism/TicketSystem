using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Api.Dtos.Messages;

public class CreateMessageDto
{
    [Required(ErrorMessage = "Content Is Required")]
    [MinLength(1, ErrorMessage = "Content Is Too Short")]
    [MaxLength(2000, ErrorMessage = "Content Is Too Long")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "TicketId Is Required")]
    public Guid TicketId { get; set; }
}
