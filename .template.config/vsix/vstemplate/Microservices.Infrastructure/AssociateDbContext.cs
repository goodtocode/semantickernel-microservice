
using Microsoft.EntityFrameworkCore;
using Microservice.Domain;
using System;

namespace $safeprojectname$
{
    public partial class AssociateDbContext : DbContext, IAssociateDbContext
    {
        public AssociateDbContext(DbContextOptions<AssociateDbContext> options)
            : base(options)
        { }

        public virtual DbSet<Business> Business { get; set; }
        public virtual DbSet<Associate> Associate { get; set; }        
        public virtual DbSet<Gender> Gender { get; set; }
        public virtual DbSet<Government> Government { get; set; }
        public virtual DbSet<Person> Person { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Add Sql connection string to the MICROSERVICE_SQL_CONNECTION environment variable to run CLI migrations
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("MICROSERVICE_SQL_CONNECTION"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Business>(entity =>
            {
                entity.ToTable("Business", "Microservices");
                entity.HasIndex(e => e.BusinessKey)
                    .HasName("IX_Business_Key")
                    .IsUnique();

                entity.Property(e => e.BusinessName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.TaxNumber)
                    .HasMaxLength(20);

            });

            modelBuilder.Entity<Associate>(entity =>
            {
                entity.ToTable("Associate", "Microservices");

                entity.HasIndex(e => e.AssociateKey)
                    .HasName("IX_AssociateLocation_Associate")
                    .IsUnique();

            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender", "Microservices");

                entity.HasIndex(e => e.GenderCode)
                    .HasName("IX_Gender_Code")
                    .IsUnique();

                entity.HasCheckConstraint("CC_Gender_GenderCode", "GenderCode in ('M', 'F', 'N/A', 'U/K')"); // ISO/IEC 5218

                entity.HasIndex(e => e.GenderKey)
                    .HasName("IX_Gender_Key")
                    .IsUnique();

                entity.Property(e => e.GenderCode).ValueGeneratedNever();

                

                entity.Property(e => e.GenderCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.GenderName)
                    .IsRequired()
                    .HasMaxLength(50);

                
            });

            modelBuilder.Entity<Government>(entity =>
            {
                entity.ToTable("Government", "Microservices");

                entity.HasIndex(e => e.GovernmentKey)
                    .HasName("IX_Government_Associate")
                    .IsUnique();

                

                entity.Property(e => e.GovernmentName)
                    .IsRequired()
                    .HasMaxLength(50);

                
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person", "Microservices");

                entity.HasIndex(e => e.PersonKey)
                    .HasName("IX_Person_Associate")
                    .IsUnique();

                entity.HasIndex(e => new { e.FirstName, e.MiddleName, e.LastName, e.BirthDate })
                    .HasName("IX_Person_All");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MiddleName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.GenderCode).HasMaxLength(3);
                entity.HasCheckConstraint("CC_Person_GenderCode", "GenderCode in ('M', 'F', 'N/A', 'U/K')"); // ISO/IEC 5218
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
