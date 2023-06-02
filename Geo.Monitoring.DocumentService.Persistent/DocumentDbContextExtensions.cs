using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Geo.Monitoring.DocumentService.Persistent;

public static class DocumentDbContextExtensions
{
    public static void AddDocumentDbContext(this IServiceCollection service, DocumentDbOptions options)
    {
        service.AddDbContext<DocumentDbContext>(option =>
        {
            option
                .UseMySql(options.ConnectionString, MySqlServerVersion.LatestSupportedServerVersion, opt =>
                {
                    //opt.MigrationsHistoryTable(DocumentDbContext.MigrationTableName, DocumentDbContext.DbSchema);
                    opt.CommandTimeout(options.CommandTimeoutSecond);
                })
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });
    }
}