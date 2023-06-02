﻿// <auto-generated />
using Geo.Monitoring.DocumentService.Persistent;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Geo.Monitoring.DocumentService.Persistent.Migrations
{
    [DbContext(typeof(DocumentDbContext))]
    [Migration("20230602150423_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Geo.Monitoring.DocumentService.Domain.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("MEDIUMBLOB");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("TINYTEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExternalId")
                        .HasMaxLength(255)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("Name")
                        .HasColumnType("TINYTEXT");

                    b.HasKey("Id");

                    b.HasIndex("ExternalId");

                    b.ToTable("Documents");
                });
#pragma warning restore 612, 618
        }
    }
}
