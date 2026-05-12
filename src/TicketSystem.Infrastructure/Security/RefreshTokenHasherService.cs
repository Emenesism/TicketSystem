using System.Security.Cryptography;
using System.Text;

namespace TicketSystem.Infrastructure.Security;

public sealed class RefreshTokenHasher : IRefreshTokenHasher
{
    public string Hash(string refreshToken)
    {
        var bytes = Encoding.UTF8.GetBytes(refreshToken);
        var hashBytes = SHA256.HashData(bytes);

        return Convert.ToBase64String(hashBytes);
    }
}
