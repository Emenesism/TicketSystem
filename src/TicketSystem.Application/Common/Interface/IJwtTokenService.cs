using TicketSystem.Domain.Entities;

namespace TicketSystem.Application.Common.Interface;


public interface IJwtTokenService

{
    string GenerateUserToken(User user);
    string GenerateAdminToken(Admin admin);
}
