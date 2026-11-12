using Microsoft.EntityFrameworkCore;
using ProductInventoryApi.Entities;
using System;

namespace ProductInventoryApi.Data
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
                entity.HasIndex(p => p.PodructCode).IsUnique();
                entity.Property(p => p.PodructCode).HasMaxLength(50).IsRequired();
                entity.Property(p => p.Name).HasMaxLength(150).IsRequired();
                entity.Property(p => p.Description).HasMaxLength(500);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(p => p.Stock).IsRequired();
            });
        }
    }
}
