
using System.ComponentModel.DataAnnotations;
namespace TicketSystem.Application.Dtos.Messages;


public class MessageIdDto
{
    [Required(ErrorMessage = "MessageId Is Required")]
    public Guid MessageId { get; set; }
}
