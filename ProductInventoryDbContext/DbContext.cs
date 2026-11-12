using Microsoft.EntityFrameworkCore;
using ProductInventory.Entities;
using System;

namespace ProductInventory.Data
{
    public class ApiDBContext : DbContext
    {
        public ApiDBContext(DbContextOptions<ApiDBContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.Id);
                entity.HasIndex(p => p.ProductCode).IsUnique();
                entity.Property(p => p.ProductCode).HasMaxLength(20).IsRequired();
                entity.Property(p => p.Name).HasMaxLength(200).IsRequired();
                entity.Property(p => p.Description).HasMaxLength(3000);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(p => p.Stock).IsRequired();
            });
            /*
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ProductCode).IsUnique();
                entity.Property(e => e.ProductCode).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(3000);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });
            */
        }
    }
}
