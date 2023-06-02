using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Geo.Monitoring.DocumentService.Persistent;

public class DocumentDbContextFactory : IDesignTimeDbContextFactory<DocumentDbContext>
{
    public DocumentDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DocumentDbContext>();
        optionsBuilder.UseMySql("server=localhost;port=3306;uid=geotest;pwd=geotest;database=geotest", MySqlServerVersion.LatestSupportedServerVersion, opt =>
        {
            //opt.MigrationsHistoryTable(DocumentDbContext.MigrationTableName, DocumentDbContext.DbSchema);
        });

        return new DocumentDbContext(optionsBuilder.Options);
    }
}