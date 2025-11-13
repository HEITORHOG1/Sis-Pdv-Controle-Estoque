using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories.Map
{
    public class MapPayment : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .IsRequired()
                .HasColumnName("Id");

            builder.Property(p => p.OrderId)
                .IsRequired()
                .HasColumnName("OrderId");

            builder.Property(p => p.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasColumnName("TotalAmount");

            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName("Status");

            builder.Property(p => p.PaymentDate)
                .IsRequired()
                .HasColumnName("PaymentDate");

            builder.Property(p => p.TransactionId)
                .HasMaxLength(100)
                .HasColumnName("TransactionId");

            builder.Property(p => p.AuthorizationCode)
                .HasMaxLength(50)
                .HasColumnName("AuthorizationCode");

            builder.Property(p => p.ProcessorResponse)
                .HasMaxLength(1000)
                .HasColumnName("ProcessorResponse");

            builder.Property(p => p.ErrorMessage)
                .HasMaxLength(500)
                .HasColumnName("ErrorMessage");

            builder.Property(p => p.ProcessedAt)
                .HasColumnName("ProcessedAt");

            // Audit fields
            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(p => p.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder.Property(p => p.CreatedBy)
                .HasColumnName("CreatedBy");

            builder.Property(p => p.UpdatedBy)
                .HasColumnName("UpdatedBy");

            builder.Property(p => p.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(p => p.DeletedAt)
                .HasColumnName("DeletedAt");

            builder.Property(p => p.DeletedBy)
                .HasColumnName("DeletedBy");

            // Relationships
            builder.HasOne(p => p.Order)
                .WithMany()
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.PaymentItems)
                .WithOne(pi => pi.Payment)
                .HasForeignKey(pi => pi.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.FiscalReceipt)
                .WithOne(fr => fr.Payment)
                .HasForeignKey<FiscalReceipt>(fr => fr.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(p => p.OrderId)
                .HasDatabaseName("IX_Payments_OrderId");

            builder.HasIndex(p => p.Status)
                .HasDatabaseName("IX_Payments_Status");

            builder.HasIndex(p => p.PaymentDate)
                .HasDatabaseName("IX_Payments_PaymentDate");

            builder.HasIndex(p => p.TransactionId)
                .HasDatabaseName("IX_Payments_TransactionId");

            // Query filter for soft delete
            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}