using System.Security.Cryptography;
using TicketSystem.Application.Common.Interface;

namespace TicketSystem.Infrastructure.Security;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(randomBytes);
    }
}
