using Microsoft.EntityFrameworkCore;
using TicketSystem.Application.Abstractions.Repositories;
using TicketSystem.Domain.Entities;
using TicketSystem.Infrastructure.Persistance.Configuration;

namespace TicketSystem.Infrastructure.Persistance.Repositories;

public class AdminRepo(AppDbContext db) : IAdminRepository
{
    private readonly AppDbContext _db = db;

    public async Task CreateAdmin(Admin admin)
    {
        await _db.Admins.AddAsync(admin);
        await _db.SaveChangesAsync();
    }

    public async Task<Admin?> IsAdmin(Guid id)
    {
        return await _db.Admins
        .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Admin>> GetAllAdmins()
    {
        return await _db.Admins
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task<Admin?> GetAdminByUsername(string username)
    {
        return await _db.Admins
        .AsNoTracking()
        .FirstOrDefaultAsync(s => s.Username == username);
    }



}
