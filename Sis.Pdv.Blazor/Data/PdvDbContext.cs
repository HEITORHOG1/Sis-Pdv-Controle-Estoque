using Microsoft.EntityFrameworkCore;
using Sis.Pdv.Blazor.Data.Entities;

namespace Sis.Pdv.Blazor.Data;

/// <summary>
/// DbContext do banco local do PDV.
/// Banco separado do sistema de gestão, sincronizado via RabbitMQ.
/// </summary>
public sealed class PdvDbContext : DbContext
{
    public PdvDbContext(DbContextOptions<PdvDbContext> options)
        : base(options) { }

    public DbSet<ProdutoEntity> Produtos => Set<ProdutoEntity>();
    public DbSet<VendaEntity> Vendas => Set<VendaEntity>();
    public DbSet<ItemVendaEntity> ItensVenda => Set<ItemVendaEntity>();
    public DbSet<UsuarioLocalEntity> Usuarios => Set<UsuarioLocalEntity>();
    public DbSet<VendaRascunhoEntity> VendasRascunho => Set<VendaRascunhoEntity>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Produto: índice único no código de barras
        modelBuilder.Entity<ProdutoEntity>(entity =>
        {
            entity.HasIndex(p => p.CodBarras)
                  .IsUnique()
                  .HasDatabaseName("IX_Produtos_CodBarras");

            entity.HasIndex(p => p.NomeProduto)
                  .HasDatabaseName("IX_Produtos_Nome");
        });

        // Venda: índice para busca por data e sync
        modelBuilder.Entity<VendaEntity>(entity =>
        {
            entity.HasIndex(v => v.DataVenda)
                  .HasDatabaseName("IX_Vendas_DataVenda");

            entity.HasIndex(v => v.Sincronizada)
                  .HasDatabaseName("IX_Vendas_Sincronizada");

            entity.HasMany(v => v.Itens)
                  .WithOne(i => i.Venda)
                  .HasForeignKey(i => i.VendaId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Item: índice por venda
        modelBuilder.Entity<ItemVendaEntity>(entity =>
        {
            entity.HasIndex(i => i.VendaId)
                  .HasDatabaseName("IX_ItensVenda_VendaId");
        });

        // Usuário: índice no Login
        modelBuilder.Entity<UsuarioLocalEntity>(entity =>
        {
            entity.HasIndex(u => u.Login)
                  .IsUnique()
                  .HasDatabaseName("IX_Usuarios_Login");
        });
    }
}
