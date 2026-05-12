using TicketSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Infrastructure.Persistance.Configuration;

public class SessionRepo(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public async Task CreateSession(Session session)
    {
        await _db.Sessions.AddAsync(session);
        await _db.SaveChangesAsync();
    }

    public async Task<Session?> GetSessionByUserId(Guid userId)
    {
        return await _db.Sessions.AsNoTracking()
        .Where(s => s.UserId == userId &&
               s.ExpiresAt > DateTime.UtcNow &&
               s.RevokeAt == null)
        .OrderByDescending(s => s.CreatedAt)
        .FirstOrDefaultAsync();
    }

    public async Task<Session?> GetSessionByAdminId(Guid adminId)
    {
        return await _db.Sessions.AsNoTracking()
        .Where(s => s.AdminId == adminId &&
        s.ExpiresAt > DateTime.UtcNow &&
        s.RevokeAt == null)
        .OrderByDescending(s => s.CreatedAt)
        .FirstOrDefaultAsync();

    }

    public async Task<Session?> GetSessionBySessionId(Guid id)
    {
        return await _db.Sessions.AsNoTracking()
        .FirstOrDefaultAsync(
            s => s.Id == id &&
            s.ExpiresAt > DateTime.UtcNow &&
            s.RevokeAt == null);
    }

    public async Task<bool> RevokeSession(Guid id)
    {
        var sessionFromDb = await _db.Sessions
        .FirstOrDefaultAsync(s => s.Id == id &&
            s.RevokeAt == null);

        if (sessionFromDb is null)
        {
            return false;
        }
        sessionFromDb.RevokeAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<Session?> GetSessionByHashToken(string tokenHash)
    {
        return await _db.Sessions
        .AsNoTracking()
        .FirstOrDefaultAsync(
            s => s.TokenHash == tokenHash &&
            s.RevokeAt == null &&
            s.ExpiresAt > DateTime.UtcNow);
    }
}
