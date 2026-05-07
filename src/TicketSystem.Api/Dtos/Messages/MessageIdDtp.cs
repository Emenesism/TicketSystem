
using System.ComponentModel.DataAnnotations;
namespace TicketSystem.Api.Dtos.Messages;

public class MessageIdDto
{
    [Required(ErrorMessage = "MessageId Is Required")]
    public Guid MessageId { get; set; }
}
