using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories.Map
{
    public class MapFiscalReceipt : IEntityTypeConfiguration<FiscalReceipt>
    {
        public void Configure(EntityTypeBuilder<FiscalReceipt> builder)
        {
            builder.ToTable("FiscalReceipts");

            builder.HasKey(fr => fr.Id);

            builder.Property(fr => fr.Id)
                .IsRequired()
                .HasColumnName("Id");

            builder.Property(fr => fr.PaymentId)
                .IsRequired()
                .HasColumnName("PaymentId");

            builder.Property(fr => fr.ReceiptNumber)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("ReceiptNumber");

            builder.Property(fr => fr.SerialNumber)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("SerialNumber");

            builder.Property(fr => fr.IssuedAt)
                .IsRequired()
                .HasColumnName("IssuedAt");

            builder.Property(fr => fr.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName("Status");

            builder.Property(fr => fr.SefazProtocol)
                .HasMaxLength(50)
                .HasColumnName("SefazProtocol");

            builder.Property(fr => fr.AccessKey)
                .HasMaxLength(44)
                .HasColumnName("AccessKey");

            builder.Property(fr => fr.QrCode)
                .HasMaxLength(500)
                .HasColumnName("QrCode");

            builder.Property(fr => fr.XmlContent)
                .HasColumnType("TEXT")
                .HasColumnName("XmlContent");

            builder.Property(fr => fr.ErrorMessage)
                .HasMaxLength(500)
                .HasColumnName("ErrorMessage");

            builder.Property(fr => fr.SentToSefazAt)
                .HasColumnName("SentToSefazAt");

            builder.Property(fr => fr.AuthorizedAt)
                .HasColumnName("AuthorizedAt");

            builder.Property(fr => fr.CancellationReason)
                .HasMaxLength(255)
                .HasColumnName("CancellationReason");

            builder.Property(fr => fr.CancelledAt)
                .HasColumnName("CancelledAt");

            // Audit fields
            builder.Property(fr => fr.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(fr => fr.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder.Property(fr => fr.CreatedBy)
                .HasColumnName("CreatedBy");

            builder.Property(fr => fr.UpdatedBy)
                .HasColumnName("UpdatedBy");

            builder.Property(fr => fr.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(fr => fr.DeletedAt)
                .HasColumnName("DeletedAt");

            builder.Property(fr => fr.DeletedBy)
                .HasColumnName("DeletedBy");

            // Relationships
            builder.HasOne(fr => fr.Payment)
                .WithOne(p => p.FiscalReceipt)
                .HasForeignKey<FiscalReceipt>(fr => fr.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(fr => fr.PaymentId)
                .IsUnique()
                .HasDatabaseName("IX_FiscalReceipts_PaymentId");

            builder.HasIndex(fr => fr.ReceiptNumber)
                .IsUnique()
                .HasDatabaseName("IX_FiscalReceipts_ReceiptNumber");

            builder.HasIndex(fr => fr.AccessKey)
                .IsUnique()
                .HasDatabaseName("IX_FiscalReceipts_AccessKey");

            builder.HasIndex(fr => fr.Status)
                .HasDatabaseName("IX_FiscalReceipts_Status");

            builder.HasIndex(fr => fr.IssuedAt)
                .HasDatabaseName("IX_FiscalReceipts_IssuedAt");

            // Query filter for soft delete
            builder.HasQueryFilter(fr => !fr.IsDeleted);
        }
    }
}