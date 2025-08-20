using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace Repositories.Map
{
    public class MapStockMovementDetail : IEntityTypeConfiguration<StockMovementDetail>
    {
        public void Configure(EntityTypeBuilder<StockMovementDetail> builder)
        {
            builder.ToTable("StockMovementDetails");
            
            // Primary Key
            builder.HasKey(x => x.Id);
            
            // Properties
            builder.Property(x => x.StockMovementId).IsRequired();
            builder.Property(x => x.Lote).HasMaxLength(50);
            builder.Property(x => x.DataValidade);
            builder.Property(x => x.Quantity).HasPrecision(18, 4).IsRequired();
            
            // Relationships
            builder.HasOne(x => x.StockMovement)
                   .WithMany(x => x.MovementDetails)
                   .HasForeignKey(x => x.StockMovementId)
                   .OnDelete(DeleteBehavior.Cascade);
            
            // Indexes
            builder.HasIndex(x => x.StockMovementId);
            builder.HasIndex(x => x.Lote);
            builder.HasIndex(x => x.DataValidade);
        }
    }
}