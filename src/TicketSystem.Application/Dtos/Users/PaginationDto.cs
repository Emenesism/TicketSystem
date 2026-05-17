
using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Application.Dtos.Users;

public class PaginationDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Page Must Be At Least 1")]
    public int Page { get; set; } = 1; //Default Value For Page
    [Range(1, int.MaxValue, ErrorMessage = "Limit Must Be At Least 1")]
    public int Limit { get; set; } = 10; //Default Value For Limit

}
