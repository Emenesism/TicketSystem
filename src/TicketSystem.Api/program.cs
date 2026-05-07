
using Microsoft.EntityFrameworkCore;
using TicketSystem.Application.Common.Interface;
using TicketSystem.Infrastructure.Security;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TicketSystem.Infrastructure.Persistance.Configuration;
using TicketSystem.Application.Abstractions.Repositories;
using TicketSystem.Infrastructure.Persistance.Repositories;
using Microsoft.AspNetCore.Components.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(conn);
});

builder.Services.AddScoped<IPasswordHasher, PassowordHasherService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IUserRepository, UserRepo>();
builder.Services.AddScoped<IAdminRepository, AdminRepo>();
builder.Services.AddScoped<ITicketRepository, TicketRepo>();
builder.Services.AddScoped<ITicketMessageRepository, TicketMessageRepo>();

var jwt = builder.Configuration.GetSection("Jwt");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!)
            ),

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? ["http://localhost:8080", "http://127.0.0.1:8080"];
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.AllowAnyOrigin()
      .AllowAnyHeader()
      .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("DefaultPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
await app.ApplyAllMigrateAsync();

app.Run();
