using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TicketSystem.Application.Common.Interface;
using TicketSystem.Domain.Entities;

namespace TicketSystem.Infrastructure.Security;

public sealed class JwtTokenService(IConfiguration config) : IJwtTokenService
{
    private readonly IConfiguration _config = config;

    public string GenerateUserToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "User")
        };

        return GenerateToken(claims);
    }

    public string GenerateAdminToken(Admin admin)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
            new Claim(ClaimTypes.Name, admin.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        return GenerateToken(claims);
    }

    private string GenerateToken(IEnumerable<Claim> claims)
    {
        var jwt = _config.GetSection("Jwt");

        var secret = jwt["Key"];
        var issuer = jwt["Issuer"];
        var audience = jwt["Audience"];
        var expiresMinutes = jwt["ExpiresMinutes"];

        if (string.IsNullOrWhiteSpace(secret))
            throw new InvalidOperationException("JWT Secret is missing.");

        if (string.IsNullOrWhiteSpace(issuer))
            throw new InvalidOperationException("JWT Issuer is missing.");

        if (string.IsNullOrWhiteSpace(audience))
            throw new InvalidOperationException("JWT Audience is missing.");

        if (string.IsNullOrWhiteSpace(expiresMinutes))
            throw new InvalidOperationException("JWT ExpiresMinutes is missing.");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secret)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(expiresMinutes)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
