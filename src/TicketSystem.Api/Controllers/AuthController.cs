using Microsoft.AspNetCore.Mvc;
using TicketSystem.Application.Dtos.Admins;
using TicketSystem.Application.Dtos.Users;
using TicketSystem.Application.Abstractions.Repositories;
using TicketSystem.Application.Common.Interface;
using TicketSystem.Domain.Entities;
using TicketSystem.Application.Dtos.Auth;
using Microsoft.AspNetCore.Authorization;
using TicketSystem.Infrastructure.Security;

namespace TicketSystem.Api.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController(IUserRepository userRepository, IAdminRepository adminRepository, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, IRefreshTokenGenerator refreshTokenGenerator, ISessionRepo sessionRepo, IRefreshTokenHasher refreshTokenHasher) : ControllerBase
{
    [HttpPost("user/login")]
    public async Task<ActionResult<CreateUserReponse>> LoginOrRegisterUser([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var createdRefreshToken = refreshTokenGenerator.GenerateRefreshToken();
        var createdRefreshTokenHash = refreshTokenHasher.Hash(createdRefreshToken);

        var userFromDb = await userRepository.GetUserByUsername(dto.Username);

        if (userFromDb is null)
        {
            var passwordHash = passwordHasher.Hash(dto.Password);
            var newUser = new User(dto.Name, dto.Username, passwordHash);

            await userRepository.CreateUser(newUser);

            var createdToken = jwtTokenService.GenerateUserToken(newUser);

            await CreateUserSession(createdRefreshTokenHash, newUser.Id);

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

        await CreateUserSession(createdRefreshTokenHash, userFromDb.Id);


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
        var createdRefreshTokenHash = refreshTokenHasher.Hash(createdRefreshToken);

        var adminFromDb = await adminRepository.GetAdminByUsername(dto.Username);

        if (adminFromDb is null)
        {
            var passwordHash = passwordHasher.Hash(dto.Password);

            var admin = new Admin(dto.Name, dto.Username, passwordHash);

            await adminRepository.CreateAdmin(admin);

            var createdToken = jwtTokenService.GenerateAdminToken(admin);

            await CreateAdminSession(createdRefreshTokenHash, admin.Id);


            return Ok(new CreateAdminReponse
            {
                Id = admin.Id,
                Name = admin.Name,
                Username = admin.Username,
                Token = createdToken,
                RefreshToken = createdRefreshToken,
                CreatedAt = admin.CreatedAt
            });

        }

        var isValid = passwordHasher.Verify(adminFromDb.PasswordHash, dto.Password);

        if (!isValid)
        {
            return Unauthorized(new { Message = "Password Or Username Is Wrong." });
        }

        var token = jwtTokenService.GenerateAdminToken(adminFromDb);


        await CreateAdminSession(createdRefreshTokenHash, adminFromDb.Id);

        return Ok(new CreateAdminReponse
        {
            Id = adminFromDb.Id,
            Name = adminFromDb.Name,
            Username = adminFromDb.Username,
            Token = token,
            RefreshToken = createdRefreshToken,
            CreatedAt = adminFromDb.CreatedAt
        });
    }

    [HttpPost("admin/refresh")]
    public async Task<ActionResult<RefreshTokenResponse>> RenewAdminAccessToken([FromBody] RefreshTokenDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var refreshTokenHash = refreshTokenHasher.Hash(dto.RefreshToken);

        var session = await sessionRepo.GetSessionByHashToken(refreshTokenHash);

        if (session is null)
        {
            return Unauthorized(new
            {
                message = "Refresh Token Is Not Valid."
            });
        }

        if (session.AdminId is null || !session.IsAdmin)
        {
            return Unauthorized(new
            {
                message = "You Are In Wrong Place Dude."
            });
        }

        var adminFromDb = await adminRepository.GetAdminById(session.AdminId.Value);

        if (adminFromDb is null)
        {
            return Unauthorized(new
            {
                message = "You Are In Wrong Place Dude."
            });
        }

        var revokeStatus = await sessionRepo.RevokeSession(session.Id);

        if (!revokeStatus)
        {
            return Unauthorized(new
            {
                message = "Failed To Revoke Old Session."
            });
        }

        var newAccessToken = jwtTokenService.GenerateAdminToken(adminFromDb);
        var newRefreshToken = refreshTokenGenerator.GenerateRefreshToken();
        var newRefreshTokenHash = refreshTokenHasher.Hash(newRefreshToken);

        await CreateAdminSession(newRefreshTokenHash, session.AdminId.Value);

        return Ok(new RefreshTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
        });

    }

    [HttpPost("user/refresh")]
    public async Task<ActionResult<RefreshTokenResponse>> RenewUserAccessToken([FromBody] RefreshTokenDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var refreshTokenHash = refreshTokenHasher.Hash(dto.RefreshToken);

        var session = await sessionRepo.GetSessionByHashToken(refreshTokenHash);

        if (session is null)
        {
            return Unauthorized(new
            {
                message = "Refresh Token Is Not Valid."
            });
        }

        if (session.UserId is null || session.IsAdmin)
        {
            return Unauthorized(new
            {
                message = "You Are In Wrong Place Dude."
            });
        }

        var userFromDb = await userRepository.GetUserById(session.UserId.Value);

        if (userFromDb is null)
        {
            return Unauthorized(new
            {
                message = "You Are In Wrong Place Dude."
            });
        }

        var revokeStatus = await sessionRepo.RevokeSession(session.Id);

        if (!revokeStatus)
        {
            return Unauthorized(new
            {
                message = "Failed To Revoke Old Session."
            });
        }

        var newAccessToken = jwtTokenService.GenerateUserToken(userFromDb);
        var newRefreshToken = refreshTokenGenerator.GenerateRefreshToken();
        var newRefreshTokenHash = refreshTokenHasher.Hash(newRefreshToken);

        await CreateUserSession(newRefreshTokenHash, session.UserId.Value);

        return Ok(new RefreshTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
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
