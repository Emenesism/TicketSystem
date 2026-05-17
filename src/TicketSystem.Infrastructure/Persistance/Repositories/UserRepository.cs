using TicketSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Application.Abstractions.Repositories;
using TicketSystem.Infrastructure.Persistance.Configuration;

namespace TicketSystem.Infrastructure.Persistance.Repositories;


public class UserRepo(AppDbContext db) : IUserRepository
{
    private readonly AppDbContext _db = db;

    public async Task CreateUser(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task<User?> GetUserByName(string name)
    {
        return await _db.Users.FirstOrDefaultAsync(s => s.Name == name);

    }

    public async Task<List<User>> GetUsersAsync(int page, int limit)
    {
        return await _db.Users
        .Where(s => s.IsDeleted == false)
        .OrderBy(s => s.CreatedAt)
        .Skip((page - 1) * limit)
        .Take(limit)
        .ToListAsync();
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _db.Users
        .FirstOrDefaultAsync(s => s.Username == username);
    }

    public async Task<List<User>> GetAllUserAfterCertianDate(DateTime date, int page, int limit)
    {
        return await _db.Users
        .AsNoTracking()
        .Where(s =>
            s.CreatedAt >= date &&
            s.IsDeleted == false)
        .OrderBy(s => s.CreatedAt)
        .Skip((page - 1) * limit)
        .Take(limit)
        .ToListAsync();
    }

    public async Task<List<User>> GetAllUserBeforeCertianDate(DateTime date, int page, int limit)
    {
        return await _db.Users
        .AsNoTracking()
        .Where(s =>
            s.CreatedAt <= date &&
            s.IsDeleted == false)
        .OrderBy(s => s.CreatedAt)
        .Skip((page - 1) * limit)
        .Take(limit)
        .ToListAsync();
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);
    }


    public async Task<int> GetTotalUserCount()
    {
        return await _db.Users.AsNoTracking().CountAsync(s => s.IsDeleted == false);
    }


    public async Task<int> GetTotalUserCountAfterDate(DateTime date)
    {
        return await _db.Users.AsNoTracking().CountAsync(s => s.IsDeleted == false && s.CreatedAt >= date);
    }

    public async Task<int> GetTotalUserCountBeforeDate(DateTime date)
    {
        return await _db.Users.AsNoTracking().CountAsync(s => s.IsDeleted == false && s.CreatedAt <= date);
    }


}
