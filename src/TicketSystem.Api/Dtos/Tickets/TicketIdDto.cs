


using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Api.Dtos.Tickets;

public class TicketIdDto
{

    [Required(ErrorMessage = "TicketId Is Required")]
    public Guid TicketId { get; set; }
}
