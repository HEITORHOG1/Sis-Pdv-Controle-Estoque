using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories.Map
{
    public class MapPaymentItem : IEntityTypeConfiguration<PaymentItem>
    {
        public void Configure(EntityTypeBuilder<PaymentItem> builder)
        {
            builder.ToTable("PaymentItems");

            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.Id)
                .IsRequired()
                .HasColumnName("Id");

            builder.Property(pi => pi.PaymentId)
                .IsRequired()
                .HasColumnName("PaymentId");

            builder.Property(pi => pi.Method)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName("Method");

            builder.Property(pi => pi.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasColumnName("Amount");

            builder.Property(pi => pi.CardNumber)
                .HasMaxLength(20)
                .HasColumnName("CardNumber");

            builder.Property(pi => pi.CardHolderName)
                .HasMaxLength(100)
                .HasColumnName("CardHolderName");

            builder.Property(pi => pi.ProcessorTransactionId)
                .HasMaxLength(100)
                .HasColumnName("ProcessorTransactionId");

            builder.Property(pi => pi.AuthorizationCode)
                .HasMaxLength(50)
                .HasColumnName("AuthorizationCode");

            builder.Property(pi => pi.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName("Status");

            builder.Property(pi => pi.ProcessedAt)
                .HasColumnName("ProcessedAt");

            builder.Property(pi => pi.ErrorMessage)
                .HasMaxLength(500)
                .HasColumnName("ErrorMessage");

            // Audit fields
            builder.Property(pi => pi.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(pi => pi.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder.Property(pi => pi.CreatedBy)
                .HasColumnName("CreatedBy");

            builder.Property(pi => pi.UpdatedBy)
                .HasColumnName("UpdatedBy");

            builder.Property(pi => pi.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(pi => pi.DeletedAt)
                .HasColumnName("DeletedAt");

            builder.Property(pi => pi.DeletedBy)
                .HasColumnName("DeletedBy");

            // Relationships
            builder.HasOne(pi => pi.Payment)
                .WithMany(p => p.PaymentItems)
                .HasForeignKey(pi => pi.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(pi => pi.PaymentId)
                .HasDatabaseName("IX_PaymentItems_PaymentId");

            builder.HasIndex(pi => pi.Method)
                .HasDatabaseName("IX_PaymentItems_Method");

            builder.HasIndex(pi => pi.Status)
                .HasDatabaseName("IX_PaymentItems_Status");

            // Query filter for soft delete
            builder.HasQueryFilter(pi => !pi.IsDeleted);
        }
    }
}