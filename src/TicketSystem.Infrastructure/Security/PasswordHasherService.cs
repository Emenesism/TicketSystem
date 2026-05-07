using Microsoft.AspNetCore.Identity;
using TicketSystem.Application.Common.Interface;


namespace TicketSystem.Infrastructure.Security;


public sealed class PassowordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string Hash(string password)
    {
        return _passwordHasher.HashPassword(null!, password);
    }

    public bool Verify(string hash, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(null!, hash, password);

        return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
    }
}
