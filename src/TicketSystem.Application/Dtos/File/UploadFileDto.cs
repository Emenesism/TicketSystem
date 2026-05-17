using System.ComponentModel.DataAnnotations;


namespace TicketSystem.Application.Dtos.File;

public class UploadFileDto
{
    [Required(ErrorMessage = "File name is required.")]
    public string FileName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content type is required.")]
    public string ContentType { get; set; } = string.Empty;
}
