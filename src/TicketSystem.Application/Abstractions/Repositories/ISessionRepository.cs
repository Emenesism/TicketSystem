using TicketSystem.Domain.Entities;

namespace TicketSystem.Application.Abstractions.Repositories;

public interface ISessionRepo
{
    Task CreateSession(Session session);

    Task<Session?> GetSessionByUserId(Guid userId);

    Task<Session?> GetSessionByAdminId(Guid adminId);

    Task<Session?> GetSessionBySessionId(Guid id);

    Task<bool> RevokeSession(Guid id);

    Task<Session?> GetSessionByHashToken(string tokenHash);
}
