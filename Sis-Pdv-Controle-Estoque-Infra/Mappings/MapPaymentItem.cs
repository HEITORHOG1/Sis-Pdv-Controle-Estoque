using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace Sis_Pdv_Controle_Estoque_Infra.Mappings
{
    public class MapPaymentItem : IEntityTypeConfiguration<PaymentItem>
    {
        public void Configure(EntityTypeBuilder<PaymentItem> builder)
        {
            builder.ToTable("PaymentItems");

            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(pi => pi.PaymentId)
                .HasColumnName("PaymentId")
                .IsRequired();

            builder.Property(pi => pi.Method)
                .HasColumnName("Method")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(pi => pi.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(pi => pi.CardNumber)
                .HasColumnName("CardNumber")
                .HasMaxLength(20);

            builder.Property(pi => pi.CardHolderName)
                .HasColumnName("CardHolderName")
                .HasMaxLength(100);

            builder.Property(pi => pi.ProcessorTransactionId)
                .HasColumnName("ProcessorTransactionId")
                .HasMaxLength(100);

            builder.Property(pi => pi.AuthorizationCode)
                .HasColumnName("AuthorizationCode")
                .HasMaxLength(100);

            builder.Property(pi => pi.Status)
                .HasColumnName("Status")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(pi => pi.ProcessedAt)
                .HasColumnName("ProcessedAt");

            builder.Property(pi => pi.ErrorMessage)
                .HasColumnName("ErrorMessage")
                .HasMaxLength(500);

            // Enhanced payment fields
            builder.Property(pi => pi.Installments)
                .HasColumnName("Installments")
                .HasDefaultValue(1);

            builder.Property(pi => pi.PixKey)
                .HasColumnName("PixKey")
                .HasMaxLength(100);

            builder.Property(pi => pi.PixQrCode)
                .HasColumnName("PixQrCode")
                .HasMaxLength(500);

            builder.Property(pi => pi.BankCode)
                .HasColumnName("BankCode")
                .HasMaxLength(10);

            builder.Property(pi => pi.AgencyNumber)
                .HasColumnName("AgencyNumber")
                .HasMaxLength(20);

            builder.Property(pi => pi.AccountNumber)
                .HasColumnName("AccountNumber")
                .HasMaxLength(20);

            builder.Property(pi => pi.CheckNumber)
                .HasColumnName("CheckNumber")
                .HasMaxLength(20);

            builder.Property(pi => pi.VoucherCode)
                .HasColumnName("VoucherCode")
                .HasMaxLength(50);

            builder.Property(pi => pi.InterestRate)
                .HasColumnName("InterestRate")
                .HasColumnType("decimal(5,4)");

            builder.Property(pi => pi.Fee)
                .HasColumnName("Fee")
                .HasColumnType("decimal(18,2)");

            builder.Property(pi => pi.ProcessorName)
                .HasColumnName("ProcessorName")
                .HasMaxLength(50);

            builder.Property(pi => pi.ProcessorResponse)
                .HasColumnName("ProcessorResponse")
                .HasMaxLength(1000);

            builder.Property(pi => pi.ExpiresAt)
                .HasColumnName("ExpiresAt");

            // Base entity properties
            builder.Property(pi => pi.IsDeleted)
                .HasColumnName("IsDeleted")
                .HasDefaultValue(false);

            builder.Property(pi => pi.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            builder.Property(pi => pi.UpdatedAt)
                .HasColumnName("UpdatedAt");

            // Relationships
            builder.HasOne(pi => pi.Payment)
                .WithMany(p => p.PaymentItems)
                .HasForeignKey(pi => pi.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(pi => pi.PaymentId);
            builder.HasIndex(pi => pi.Method);
            builder.HasIndex(pi => pi.Status);
            builder.HasIndex(pi => pi.ProcessorTransactionId);
        }
    }
}