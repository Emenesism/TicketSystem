using TicketSystem.Domain.Entities;

namespace TicketSystem.Application.Abstractions.Repositories;


public interface IAdminRepository
{
    Task CreateAdmin(Admin admin);
    Task<Admin?> IsAdmin(Guid id);
    Task<List<Admin>> GetAllAdmins();
    Task<Admin?> GetAdminByUsername(string username);
    Task<Admin?> GetAdminById(Guid id);
}
