using Microsoft.EntityFrameworkCore;
using TicketSystem.Domain.Entities;
using TicketSystem.Infrastructure.Persistance.Configuration;
using TicketSystem.Application.Abstractions.Repositories;

namespace TicketSystem.Infrastructure.Persistance.Repositories;


public class TicketMessageRepo(AppDbContext db) : ITicketMessageRepository
{
    private readonly AppDbContext _db = db;

    public async Task CreateTicketMessage(TicketMessage message)
    {
        await _db.TicketMessages.AddAsync(message);
        await _db.SaveChangesAsync();

    }

    public async Task<List<TicketMessage>> GetAllMessageRelatedOfTicket(Guid ticketId)
    {
        return await _db.TicketMessages
        .AsNoTracking()
        .Where(s => s.TicketId == ticketId)
        .OrderBy(s => s.CreatedAt)
        .ToListAsync();
    }


    public async Task<bool> DeleteMessage(Guid messageId, Guid senderId)
    {
        var message = await _db.TicketMessages
        .FirstOrDefaultAsync(s =>
            s.Id == messageId &&
            s.SenderId == senderId);

        if (message is null)
            return false;

        _db.TicketMessages.Remove(message);

        await _db.SaveChangesAsync();

        return true;
    }

}
