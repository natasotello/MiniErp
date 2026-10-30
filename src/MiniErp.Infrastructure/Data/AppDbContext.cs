using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MiniErp.Domain.Entities;

namespace MiniErp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.Property(p => p.Sku).IsRequired().HasMaxLength(30);
                entity.Property(p => p.Nome).IsRequired().HasMaxLength(150);
                entity.Property(p => p.PrecoVenda).HasPrecision(18, 2);
                entity.HasIndex(p => p.Sku).IsUnique();
            });
        }
    }
}
