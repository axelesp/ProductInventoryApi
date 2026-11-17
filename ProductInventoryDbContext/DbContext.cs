using Microsoft.EntityFrameworkCore;
using ProductInventory.Entities;
using ProductInventory.Entities.Entities;
using System;

namespace ProductInventory.Data
{
    public class ApiDBContext : DbContext
    {
        public ApiDBContext(DbContextOptions<ApiDBContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Categories> Categories => Set<Categories>();

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
                entity.Property(p => p.Price).HasColumnType("decimal(16,2)").IsRequired();
                entity.Property(p => p.Stock).IsRequired();
            });
            
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Description).HasMaxLength(3000);
                entity.Property(c => c.parentCategoryId).HasDefaultValue(0);
                entity.Property(c => c.IsActive).HasDefaultValue(true);
            });
        }
    }
}
