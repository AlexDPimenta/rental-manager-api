using Microsoft.EntityFrameworkCore;
using RentalManager.WebApi.Persistence.Context;

namespace RentalManager.WebApi.Common.Extension;

public static class MigrationExtension
{

    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<RentalManagerDbContext>();

        dbContext.Database.Migrate();               
    }
}
