using TicketSystem.Domain.Entities;

namespace TicketSystem.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task CreateUser(User user);
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserByName(string name);
    Task<User?> GetUserByUsername(string username);
    Task<List<User>> GetAllUserAfterCertianDate(DateTime date);
    Task<List<User>> GetAllUserBeforeCertianDate(DateTime date);
}
