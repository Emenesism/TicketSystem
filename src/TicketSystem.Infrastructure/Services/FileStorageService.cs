using TicketSystem.Application.Services;

namespace TicketSystem.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public FileStorageService(string basePath)
    {
        _basePath = basePath;
        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SaveAsync(Stream stream, string contentType, string extension)
    {
        // Generate a unique name to prevent overwrites
        var storageName = $"{Guid.NewGuid()}{extension}";
        var path = Path.Combine(_basePath, storageName);

        using var fileStream = new FileStream(path, FileMode.Create);
        await stream.CopyToAsync(fileStream);

        return storageName;
    }

    public Task DeleteAsync(string storageName)
    {
        var path = Path.Combine(_basePath, storageName);
        if (File.Exists(path))
            File.Delete(path);
        return Task.CompletedTask;
    }

    public Task<Stream> GetStreamAsync(string storageName)
    {
        var path = Path.Combine(_basePath, storageName);
        return Task.FromResult<Stream>(File.OpenRead(path));
    }
}
