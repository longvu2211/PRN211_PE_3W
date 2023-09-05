﻿using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace BusinessObjects
{
    public partial class ArtTattoo2023DBContext : DbContext
    {
        public ArtTattoo2023DBContext()
        {
        }

        public ArtTattoo2023DBContext(DbContextOptions<ArtTattoo2023DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountMember> AccountMembers { get; set; }
        public virtual DbSet<ArtTattooService> ArtTattooServices { get; set; }
        public virtual DbSet<ArtTattooStyle> ArtTattooStyles { get; set; }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
            var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];
            return strConn;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AccountMember>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK__AccountM__349DA58607EF5A4C");

                entity.ToTable("AccountMember");

                entity.HasIndex(e => e.EmailAddress, "UQ__AccountM__49A14740CAF15437")
                    .IsUnique();

                entity.Property(e => e.AccountId)
                    .ValueGeneratedNever()
                    .HasColumnName("AccountID");

                entity.Property(e => e.EmailAddress).HasMaxLength(60);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<ArtTattooService>(entity =>
            {
                entity.HasKey(e => e.ServiceId)
                    .HasName("PK__ArtTatto__C51BB00A231DF13A");

                entity.ToTable("ArtTattooService");

                entity.Property(e => e.ServiceId).HasMaxLength(30);

                entity.Property(e => e.ServiceAddress).HasMaxLength(120);

                entity.Property(e => e.ServiceName)
                    .IsRequired()
                    .HasMaxLength(90);

                entity.Property(e => e.ServiceNote)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.TelephoneNumber).HasMaxLength(15);
            });

            modelBuilder.Entity<ArtTattooStyle>(entity =>
            {
                entity.HasKey(e => e.TattooStyleId)
                    .HasName("PK__ArtTatto__C1DFE76AF22809EF");

                entity.ToTable("ArtTattooStyle");

                entity.Property(e => e.TattooStyleId).ValueGeneratedNever();

                entity.Property(e => e.ServiceId).HasMaxLength(30);

                entity.Property(e => e.StyleDescription).HasMaxLength(200);

                entity.Property(e => e.TattooLocation).HasMaxLength(80);

                entity.Property(e => e.TattooStyleName)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ArtTattooStyles)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__ArtTattoo__Servi__4E88ABD4");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
