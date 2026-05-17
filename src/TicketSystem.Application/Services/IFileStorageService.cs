namespace TicketSystem.Application.Services;


public interface IFileStorageService
{
    Task<string> SaveAsync(Stream stream, string contentType, string extension);
    Task DeleteAsync(string storageName);
    Task<Stream> GetStreamAsync(string storageName);
}
