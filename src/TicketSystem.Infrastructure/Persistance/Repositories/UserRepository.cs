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

    public async Task<List<User>> GetUsersAsync()
    {
        return await _db.Users.Where(s => s.IsDeleted == false).ToListAsync();
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _db.Users.FirstOrDefaultAsync(s => s.Username == username);
    }

    public async Task<List<User>> GetAllUserAfterCertianDate(DateTime date)
    {
        return await _db.Users.AsNoTracking().Where(s => s.CreatedAt >= date).OrderBy(s => s.CreatedAt).ToListAsync();
    }

    public async Task<List<User>> GetAllUserBeforeCertianDate(DateTime date)
    {
        return await _db.Users.AsNoTracking().Where(s => s.CreatedAt <= date).OrderBy(s => s.CreatedAt).ToListAsync();
    }


}
