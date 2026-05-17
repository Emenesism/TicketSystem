using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Application.Dtos.Users;


public class GetUsersFilterDto : PaginationDto
{
    [Required(ErrorMessage = "Date is required")]
    public DateTime Date { get; set; }
}
