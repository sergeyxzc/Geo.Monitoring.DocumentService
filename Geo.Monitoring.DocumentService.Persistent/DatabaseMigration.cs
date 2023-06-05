using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Geo.Monitoring.DocumentService.Persistent;

public static class DatabaseMigration
{
    public static async Task TryMigrateAsync(IServiceCollection services, DocumentDbOptions dbOptions)
    {
        await using var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();

        var migration = scope.ServiceProvider.GetRequiredService<DocumentDatabaseMigration>();

        await migration.TryMigrateAsync(dbOptions);
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

    public async Task TryMigrateAsync(DocumentDbOptions dbOptions)
    {
        try
        {
            if (dbOptions.MigrationEnabled)
            {
                await _context.Database.MigrateAsync();
                _logger.LogInformation("Migrations applied");
            }
            else
            {
                _logger.LogInformation("Applying migration is disabled");
            }
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Unable to apply migrations");
            throw;
        }
    }
}