using Microsoft.EntityFrameworkCore;
using TicketSystem.Domain.Entities;
using TicketSystem.Infrastructure.Persistance.Configuration;
using TicketSystem.Application.Abstractions.Repositories;




namespace TicketSystem.Infrastructure.Persistance.Repositories;

public class TicketRepo(AppDbContext db) : ITicketRepository
{
    private readonly AppDbContext _db = db;

    public async Task CreateTicket(Ticket ticket)
    {
        await _db.Tickets.AddAsync(ticket);
        await _db.SaveChangesAsync();
    }

    public async Task<Ticket?> GetTicketViaTicketId(Guid ticketId)
    {
        return await _db.Tickets
        .AsNoTracking()
        .FirstOrDefaultAsync(s => s.Id == ticketId);
    }

    public async Task<List<Ticket>> GetAllUserTicket(Guid userId)
    {
        return await _db.Tickets
        .AsNoTracking()
        .Where(s => s.UserId == userId)
        .ToListAsync();
    }

    public async Task<List<Ticket>> GetAllAdminTicket(Guid adminId)
    {
        return await _db.Tickets.AsNoTracking()
        .Where(s => s.AdminId == adminId)
        .ToListAsync();
    }

    public async Task<List<Ticket>> GetAllNotAssingedTicket()
    {
        return await _db.Tickets
        .AsNoTracking()
        .Where(s => s.AdminId == null)
        .ToListAsync();
    }

    public async Task<bool> AssignTicketToAdmin(Guid ticketId, Guid adminId)
    {
        var ticket = await _db.Tickets
        .FirstOrDefaultAsync(s =>
            s.Id == ticketId &&
            s.AdminId == null);

        if (ticket is null)
        {
            return false;
        }

        ticket.AdminId = adminId;

        await _db.SaveChangesAsync();

        return true;

    }

    public async Task<bool> ChangeStatusOfSolveToTrue(Guid ticketId, Guid adminId)
    {
        var ticket = await _db.Tickets
        .FirstOrDefaultAsync(s =>
            s.Id == ticketId &&
            s.AdminId == adminId);

        if (ticket is null)
        {
            return false;
        }

        ticket.Solved = true;

        await _db.SaveChangesAsync();
        return true;
    }

}
