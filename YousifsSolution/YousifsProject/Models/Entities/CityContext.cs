using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace YousifsProject.Models.Entities
{
    public partial class CityContext : DbContext
    {
        public CityContext()
        {
        }

        public CityContext(DbContextOptions<CityContext> options)
            : base(options)
        {
        }

        public virtual DbSet<House> Houses { get; set; } = null!;
        public virtual DbSet<Roof> Roofs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<House>(entity =>
            {
                entity.HasIndex(e => e.Address, "UQ__Houses__7D0C3F321B2DCE52")
                    .IsUnique();

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Color).HasMaxLength(7);

                entity.Property(e => e.RoofId).HasColumnName("RoofID");

                entity.HasOne(d => d.Roof)
                    .WithMany(p => p.Houses)
                    .HasForeignKey(d => d.RoofId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Houses__RoofID__49C3F6B7");
            });

            modelBuilder.Entity<Roof>(entity =>
            {
                entity.Property(e => e.TypeOfRoof).HasMaxLength(30);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
