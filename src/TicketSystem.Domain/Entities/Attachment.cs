namespace TicketSystem.Domain.Entities;

public class Attachment(string filename, long size, string contentType)
{

    public Guid Id { get; set; } = Guid.NewGuid();
    public string Filename { get; set; } = filename;
    public long Size { get; set; } = size;
    public string ContentType { get; set; } = contentType;
    public string StorageName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    //Navigation
    public Guid TicketMessageId { get; set; }
    public TicketMessage TicketMessage { get; set; } = null!;
}
