using Geo.Monitoring.DocumentService.Persistent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;

namespace Geo.Monitoring.DocumentService;

public static class DatabaseMigration
{
    public static async Task TryMigrateAsync(IServiceCollection services)
    {
        await using var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();

        var migration = scope.ServiceProvider.GetRequiredService<DocumentDatabaseMigration>();

        await migration.TryMigrateAsync();

        //var logger = LoggerFactory.Create(config =>
        //{
        //    config.AddConsole();
        //}).CreateLogger("Migration");

        //var logger = scope.ServiceProvider.GetRequiredService<ILogger<>>();
        //await using var context = scope.ServiceProvider.GetRequiredService<DocumentDbContext>();

        //try
        //{
        //    await migration.TryMigrateAsync();
        //    logger?.LogInformation("Migrations applied");
        //}
        //catch (Exception e)
        //{
        //    logger?.LogError(e, "Unable to apply migrations");
        //    throw; 
        //}
    }
}

public class DocumentDatabaseMigration
{
    private readonly DocumentDbContext _context;
    private readonly ILogger<DocumentDatabaseMigration> _logger;

    public DocumentDatabaseMigration(DocumentDbContext context, ILogger<DocumentDatabaseMigration> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task TryMigrateAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
            _logger?.LogInformation("Migrations applied");
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Unable to apply migrations");
            throw;
        }
    }
}