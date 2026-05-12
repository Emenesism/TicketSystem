using Microsoft.AspNetCore.Mvc;
using TicketSystem.Application.Dtos.Admins;
using TicketSystem.Application.Dtos.Users;
using TicketSystem.Application.Abstractions.Repositories;
using TicketSystem.Application.Common.Interface;
using TicketSystem.Domain.Entities;
using TicketSystem.Infrastructure.Security;

namespace TicketSystem.Api.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController(IUserRepository userRepository, IAdminRepository adminRepository, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, IRefreshTokenGenerator refreshTokenGenerator, ISessionRepo sessionRepo) : ControllerBase
{
    [HttpPost("user/login")]
    public async Task<ActionResult<CreateUserReponse>> LoginOrRegisterUser([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var createdRefreshToken = refreshTokenGenerator.GenerateRefreshToken();
        var createdRefreshTokenHash = passwordHasher.Hash(createdRefreshToken);

        var userFromDb = await userRepository.GetUserByUsername(dto.Username);

        if (userFromDb is null)
        {
            var passwordHash = passwordHasher.Hash(dto.Password);
            var newUser = new User(dto.Name, dto.Username, passwordHash);

            await userRepository.CreateUser(newUser);

            var createdToken = jwtTokenService.GenerateUserToken(newUser);

            await CreateUserSession(createdRefreshToken, newUser.Id);

            return Ok(new CreateUserReponse
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Username = newUser.Username,
                Token = createdToken,
                RefreshToken = createdRefreshToken,
                CreatedAt = newUser.CreatedAt
            });
        }

        var isValidPassword = passwordHasher.Verify(userFromDb.PasswordHash, dto.Password);

        if (!isValidPassword)
        {
            return Unauthorized(new { Message = "Invalid Username Or Password." });
        }

        var token = jwtTokenService.GenerateUserToken(userFromDb);

        await CreateUserSession(createdRefreshToken, userFromDb.Id);


        return Ok(new CreateUserReponse
        {
            Id = userFromDb.Id,
            Name = userFromDb.Name,
            Username = userFromDb.Username,
            Token = token,
            RefreshToken = createdRefreshToken,
            CreatedAt = userFromDb.CreatedAt
        });
    }

    [HttpPost("admin/login")]
    public async Task<ActionResult<CreateAdminReponse>> LoginOrRegisterAdmin([FromBody] CreateAdminDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var createdRefreshToken = refreshTokenGenerator.GenerateRefreshToken();
        var createdRefreshTokenHash = passwordHasher.Hash(createdRefreshToken);

        var adminFromDb = await adminRepository.GetAdminByUsername(dto.Username);

        if (adminFromDb is null)
        {
            var passwordHash = passwordHasher.Hash(dto.Password);

            var admin = new Admin(dto.Name, dto.Username, passwordHash);

            await adminRepository.CreateAdmin(admin);

            var createdToken = jwtTokenService.GenerateAdminToken(admin);

            await CreateAdminSession(createdRefreshToken, admin.Id);


            return Ok(new CreateAdminReponse
            {
                Id = admin.Id,
                Name = admin.Name,
                Username = admin.Username,
                Token = createdToken,
                CreatedAt = admin.CreatedAt
            });

        }

        var isValid = passwordHasher.Verify(adminFromDb.PasswordHash, dto.Password);

        if (!isValid)
        {
            return Unauthorized(new { Message = "Password Or Username Is Wrong." });
        }

        var token = jwtTokenService.GenerateAdminToken(adminFromDb);

        var session_2 = new Session(
            createdRefreshTokenHash,
            adminFromDb.Id,          // adminId
            null,             // userId
            true,             // isAdmin
            DateTime.UtcNow.AddDays(30)
        );

        await sessionRepo.CreateSession(session_2);

        await CreateAdminSession(createdRefreshTokenHash, adminFromDb.Id);

        return Ok(new CreateAdminReponse
        {
            Id = adminFromDb.Id,
            Name = adminFromDb.Name,
            Username = adminFromDb.Username,
            Token = token,
            CreatedAt = adminFromDb.CreatedAt
        });
    }

    private async Task CreateUserSession(string refreshToken, Guid userId)
    {
        var session = new Session(
            refreshToken,
            null,
            userId,
            false,
            DateTime.UtcNow.AddDays(30)
        );

        await sessionRepo.CreateSession(session);
    }

    private async Task CreateAdminSession(string refreshToken, Guid adminId)
    {
        var session = new Session(
            refreshToken,
            adminId,
            null,
            true,
            DateTime.UtcNow.AddDays(30)
        );

        await sessionRepo.CreateSession(session);
    }

}
