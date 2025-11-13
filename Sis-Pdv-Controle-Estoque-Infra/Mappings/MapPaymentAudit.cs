using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace Sis_Pdv_Controle_Estoque_Infra.Mappings
{
    public class MapPaymentAudit : IEntityTypeConfiguration<PaymentAudit>
    {
        public void Configure(EntityTypeBuilder<PaymentAudit> builder)
        {
            builder.ToTable("PaymentAudits");

            builder.HasKey(pa => pa.Id);

            builder.Property(pa => pa.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(pa => pa.PaymentId)
                .HasColumnName("PaymentId")
                .IsRequired();

            builder.Property(pa => pa.Action)
                .HasColumnName("Action")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(pa => pa.Description)
                .HasColumnName("Description")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(pa => pa.PreviousData)
                .HasColumnName("PreviousData")
                .HasColumnType("TEXT");

            builder.Property(pa => pa.NewData)
                .HasColumnName("NewData")
                .HasColumnType("TEXT");

            builder.Property(pa => pa.UserId)
                .HasColumnName("UserId")
                .IsRequired();

            builder.Property(pa => pa.ActionDate)
                .HasColumnName("ActionDate")
                .IsRequired();

            builder.Property(pa => pa.IpAddress)
                .HasColumnName("IpAddress")
                .HasMaxLength(45);

            builder.Property(pa => pa.UserAgent)
                .HasColumnName("UserAgent")
                .HasMaxLength(500);

            // Base entity properties
            builder.Property(pa => pa.IsDeleted)
                .HasColumnName("IsDeleted")
                .HasDefaultValue(false);

            builder.Property(pa => pa.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired();

            builder.Property(pa => pa.UpdatedAt)
                .HasColumnName("UpdatedAt");

            // Relationships
            builder.HasOne(pa => pa.Payment)
                .WithMany()
                .HasForeignKey(pa => pa.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pa => pa.User)
                .WithMany()
                .HasForeignKey(pa => pa.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(pa => pa.PaymentId);
            builder.HasIndex(pa => pa.UserId);
            builder.HasIndex(pa => pa.Action);
            builder.HasIndex(pa => pa.ActionDate);
        }
    }
}