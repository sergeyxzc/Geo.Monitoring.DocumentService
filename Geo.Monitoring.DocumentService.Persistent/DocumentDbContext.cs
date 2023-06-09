﻿using Geo.Monitoring.DocumentService.Domain;
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

        //modelBuilder.HasDefaultSchema(DbSchema);

        // https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core.html
        modelBuilder.Entity<Document>(e =>
        {
            e.HasKey(x => x.Id);
            //e.ForMySQLHasCollation("ascii_bin"); // defining collation at Entity level
            e.Property(x => x.ContentType).HasColumnType("TINYTEXT");
            e.Property(x => x.Name).HasColumnType("TINYTEXT");
            e.Property(x => x.Description).HasColumnType("TEXT");

            // Max 16,777,215 bytes
            e.Property(x => x.Content).HasColumnType("MEDIUMBLOB");
        });

        modelBuilder.Entity<DocumentLabel>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Document)
                .WithMany(x => x.Labels)
                .HasForeignKey(x => x.DocumentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            e.Property(x => x.Label).HasColumnType("VARCHAR").HasMaxLength(255);

            e.HasIndex(x => x.Label).IsUnique(false);
        });
    }

    public DbSet<Document> Documents { get; set; }
    public DbSet<DocumentLabel> Labels { get; set; }
}