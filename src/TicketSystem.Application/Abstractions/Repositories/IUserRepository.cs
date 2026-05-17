using TicketSystem.Domain.Entities;

namespace TicketSystem.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task CreateUser(User user);
    Task<List<User>> GetUsersAsync(int page, int limit);
    Task<User?> GetUserByName(string name);
    Task<User?> GetUserByUsername(string username);
    Task<List<User>> GetAllUserAfterCertianDate(DateTime date, int page, int limit);
    Task<List<User>> GetAllUserBeforeCertianDate(DateTime date, int page, int limit);
    Task<User?> GetUserById(Guid id);
    Task<int> GetTotalUserCount();
    Task<int> GetTotalUserCountAfterDate(DateTime date);
    Task<int> GetTotalUserCountBeforeDate(DateTime date);
}
