using Microsoft.EntityFrameworkCore;
using TicketSystem.Infrastructure.Persistance.Configuration;


public static class EfCoreExtensions
{
    public static async Task ApplyAllMigrateAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();

    }
}
