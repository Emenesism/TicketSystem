using TicketSystem.Application.Dtos.File;

namespace TicketSystem.Application.Services;

public class FileValidator(int maxBytes, string[] allowedExtensions)
{
    private readonly int _maxBytes = maxBytes;
    private readonly string[] _allowedExtensions = allowedExtensions;


    public void Validate(UploadFileDto dto, long size)
    {
        if (string.IsNullOrWhiteSpace(dto.FileName))
            throw new ArgumentException("File name is required.");

        if (string.IsNullOrWhiteSpace(dto.ContentType))
            throw new ArgumentException("Content type is required.");

        if (size <= 0)
            throw new ArgumentException("File content is required.");

        if (size > _maxBytes)
            throw new ArgumentException($"File exceeds max size of {_maxBytes / 1024 / 1024} MB.");

        var extension = Path.GetExtension(dto.FileName).ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(extension) || !_allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException($"File type {extension} is not allowed.");

    }
}
