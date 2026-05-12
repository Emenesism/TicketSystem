using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Application.Dtos.Tickets;


public class TicketIdDto
{

    [Required(ErrorMessage = "TicketId Is Required")]
    public Guid TicketId { get; set; }
}
