using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace Sis_Pdv_Controle_Estoque_Infra.Mappings
{
    public class MapFiscalReceipt : IEntityTypeConfiguration<FiscalReceipt>
    {
        public void Configure(EntityTypeBuilder<FiscalReceipt> builder)
        {
            builder.ToTable("FiscalReceipts");

            builder.HasKey(fr => fr.Id);

            builder.Property(fr => fr.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(fr => fr.PaymentId)
                .HasColumnName("PaymentId")
                .IsRequired();

            builder.Property(fr => fr.ReceiptNumber)
                .HasColumnName("ReceiptNumber")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(fr => fr.SerialNumber)
                .HasColumnName("SerialNumber")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(fr => fr.IssuedAt)
                .HasColumnName("IssuedAt")
                .IsRequired();

            builder.Property(fr => fr.Status)
                .HasColumnName("Status")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(fr => fr.SefazProtocol)
                .HasColumnName("SefazProtocol")
                .HasMaxLength(100);

            builder.Property(fr => fr.AccessKey)
                .HasColumnName("AccessKey")
                .HasMaxLength(44);

            builder.Property(fr => fr.QrCode)
                .HasColumnName("QrCode")
                .HasMaxLength(500);

            builder.Property(fr => fr.XmlContent)
                .HasColumnName("XmlContent")
                .HasColumnType("TEXT");

            builder.Property(fr => fr.ErrorMessage)
                .HasColumnName("ErrorMessage")
                .HasMaxLength(500);

            builder.Property(fr => fr.SentToSefazAt)
                .HasColumnName("SentToSefazAt");

            builder.Property(fr => fr.AuthorizedAt)
                .HasColumnName("AuthorizedAt");

            builder.Property(fr => fr.CancellationReason)
                .HasColumnName("CancellationReason")
                .HasMaxLength(255);

            builder.Property(fr => fr.CancelledAt)
                .HasColumnName("CancelledAt");

            // Base entity properties
            builder.Property(fr => fr.IsDeleted)
                .HasColumnName("IsDeleted")
                .HasDefaultValue(false);

            builder.Property(fr => fr.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            builder.Property(fr => fr.UpdatedAt)
                .HasColumnName("UpdatedAt");

            // Relationships
            builder.HasOne(fr => fr.Payment)
                .WithOne(p => p.FiscalReceipt)
                .HasForeignKey<FiscalReceipt>(fr => fr.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(fr => fr.PaymentId)
                .IsUnique();
            builder.HasIndex(fr => fr.ReceiptNumber)
                .IsUnique();
            builder.HasIndex(fr => fr.AccessKey)
                .IsUnique();
            builder.HasIndex(fr => fr.Status);
            builder.HasIndex(fr => fr.IssuedAt);
        }
    }
}