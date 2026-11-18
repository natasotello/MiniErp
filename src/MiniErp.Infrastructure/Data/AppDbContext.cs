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
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<MovimentacaoEstoque> MovimentacoesEstoque { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.Property(p => p.Sku).IsRequired().HasMaxLength(30);
                entity.Property(p => p.Nome).IsRequired().HasMaxLength(150);
                entity.Property(p => p.PrecoVenda).HasPrecision(18, 2);
                entity.HasIndex(p => p.Sku).IsUnique();

                entity.HasOne(p => p.Categoria).WithMany().HasForeignKey(p => p.CategoriaId);

                entity.HasOne(p => p.Fornecedor).WithMany().HasForeignKey(p => p.FornecedorId);
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.Property(p => p.Nome).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Descricao).HasMaxLength(250);
                entity.HasIndex(p => p.Nome).IsUnique();
            });

            modelBuilder.Entity<Fornecedor>(entity =>
            {
                entity.Property(p => p.RazaoSocial).IsRequired().HasMaxLength(150);
                entity.Property(p => p.Cnpj).IsRequired().HasMaxLength(14);
                entity.HasIndex(p => p.Cnpj).IsUnique();
                entity.Property(p => p.Email).HasMaxLength(150);
                entity.Property(p => p.Telefone).HasMaxLength(20);
            });

            modelBuilder.Entity<MovimentacaoEstoque>(entity =>
            {
                entity.HasOne(m => m.Produto).WithMany()
                      .HasForeignKey(m => m.ProdutoId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.Property(m => m.Observacao).HasMaxLength(250);
            });
        }
    }
}
