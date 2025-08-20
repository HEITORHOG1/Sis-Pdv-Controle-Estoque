using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace Repositories.Map
{
    public class MapInventoryBalance : IEntityTypeConfiguration<InventoryBalance>
    {
        public void Configure(EntityTypeBuilder<InventoryBalance> builder)
        {
            builder.ToTable("InventoryBalances");
            
            // Primary Key
            builder.HasKey(x => x.Id);
            
            // Properties
            builder.Property(x => x.ProdutoId).IsRequired();
            builder.Property(x => x.CurrentStock).HasPrecision(18, 4).HasDefaultValue(0);
            builder.Property(x => x.ReservedStock).HasPrecision(18, 4).HasDefaultValue(0);
            builder.Property(x => x.MinimumStock).HasPrecision(18, 4).HasDefaultValue(0);
            builder.Property(x => x.MaximumStock).HasPrecision(18, 4).HasDefaultValue(0);
            builder.Property(x => x.ReorderPoint).HasPrecision(18, 4).HasDefaultValue(0);
            builder.Property(x => x.Location).HasMaxLength(100);
            builder.Property(x => x.LastUpdated).IsRequired();
            
            // Computed property - not mapped to database
            builder.Ignore(x => x.AvailableStock);
            
            // Relationships
            builder.HasOne(x => x.Produto)
                   .WithOne(x => x.InventoryBalance)
                   .HasForeignKey<InventoryBalance>(x => x.ProdutoId)
                   .OnDelete(DeleteBehavior.Restrict);
            
            // Indexes
            builder.HasIndex(x => x.ProdutoId).IsUnique();
            builder.HasIndex(x => x.LastUpdated);
            builder.HasIndex(x => x.Location);
        }
    }
}