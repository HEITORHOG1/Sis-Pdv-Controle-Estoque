using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace Sis_Pdv_Controle_Estoque_Infra.Mappings
{
    public class MapPayment : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(p => p.OrderId)
                .HasColumnName("OrderId")
                .IsRequired();

            builder.Property(p => p.TotalAmount)
                .HasColumnName("TotalAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.Status)
                .HasColumnName("Status")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(p => p.PaymentDate)
                .HasColumnName("PaymentDate")
                .IsRequired();

            builder.Property(p => p.TransactionId)
                .HasColumnName("TransactionId")
                .HasMaxLength(100);

            builder.Property(p => p.AuthorizationCode)
                .HasColumnName("AuthorizationCode")
                .HasMaxLength(100);

            builder.Property(p => p.ProcessorResponse)
                .HasColumnName("ProcessorResponse")
                .HasMaxLength(1000);

            builder.Property(p => p.ErrorMessage)
                .HasColumnName("ErrorMessage")
                .HasMaxLength(500);

            builder.Property(p => p.ProcessedAt)
                .HasColumnName("ProcessedAt");

            // Base entity properties
            builder.Property(p => p.IsDeleted)
                .HasColumnName("IsDeleted")
                .HasDefaultValue(false);

            builder.Property(p => p.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            builder.Property(p => p.UpdatedAt)
                .HasColumnName("UpdatedAt");

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
            builder.HasIndex(p => p.OrderId);
            builder.HasIndex(p => p.Status);
            builder.HasIndex(p => p.PaymentDate);
            builder.HasIndex(p => p.TransactionId);
        }
    }
}