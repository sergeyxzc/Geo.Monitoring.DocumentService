using Geo.Monitoring.DocumentService.Domain;
using Microsoft.EntityFrameworkCore;

namespace Geo.Monitoring.DocumentService.Persistent;

public class DocumentDbContext : DbContext
{
    //public static readonly string DbSchema = "geodoc";
    //public static readonly string MigrationTableName = "DocumentMigrations";

    public DocumentDbContext(DbContextOptions<DocumentDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core.html
        modelBuilder.Entity<Document>(e =>
        {
            e.HasKey(x => x.Id);
            //e.ForMySQLHasCollation("ascii_bin"); // defining collation at Entity level
            e.Property(x => x.ExternalId).HasColumnType("VARCHAR").HasMaxLength(255);
            e.Property(x => x.ContentType).HasColumnType("TINYTEXT");
            e.Property(x => x.Name).HasColumnType("TINYTEXT");
            e.Property(x => x.Description).HasColumnType("TEXT");
            e.Property(x => x.Content).HasColumnType("MEDIUMBLOB");

            e.HasIndex(x => x.ExternalId).IsUnique(false);
        });
        //modelBuilder.HasDefaultSchema(DbSchema);
    }

    public DbSet<Document> Documents { get; set; }
}