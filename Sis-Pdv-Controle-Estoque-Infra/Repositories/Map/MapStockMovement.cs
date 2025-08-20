using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace Repositories.Map
{
    public class MapStockMovement : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            builder.ToTable("StockMovement");
            
            // Primary Key
            builder.HasKey(x => x.Id);
            
            // Properties
            builder.Property(x => x.ProdutoId).IsRequired();
            builder.Property(x => x.Quantity).HasPrecision(18, 2).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Reason).HasMaxLength(500).IsRequired();
            builder.Property(x => x.UnitCost).HasPrecision(18, 2).IsRequired();
            builder.Property(x => x.PreviousStock).HasPrecision(18, 2).IsRequired();
            builder.Property(x => x.NewStock).HasPrecision(18, 2).IsRequired();
            builder.Property(x => x.MovementDate).IsRequired();
            builder.Property(x => x.ReferenceDocument).HasMaxLength(100);
            builder.Property(x => x.UserId);
            builder.Property(x => x.Lote).HasMaxLength(50);
            builder.Property(x => x.DataValidade);
            
            // Relationships
            builder.HasOne(x => x.Produto)
                   .WithMany(x => x.StockMovements)
                   .HasForeignKey(x => x.ProdutoId)
                   .OnDelete(DeleteBehavior.Restrict);
            
            // Relationships for MovementDetails
            builder.HasMany(x => x.MovementDetails)
                   .WithOne(x => x.StockMovement)
                   .HasForeignKey(x => x.StockMovementId)
                   .OnDelete(DeleteBehavior.Cascade);
            
            // Indexes
            builder.HasIndex(x => x.ProdutoId);
            builder.HasIndex(x => x.MovementDate);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.Lote);
            builder.HasIndex(x => x.DataValidade);
            builder.HasIndex(x => new { x.ProdutoId, x.MovementDate });
        }
    }
}