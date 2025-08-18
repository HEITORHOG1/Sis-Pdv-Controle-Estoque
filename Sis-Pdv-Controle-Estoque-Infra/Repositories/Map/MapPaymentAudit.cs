using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace Sis_Pdv_Controle_Estoque_Infra.Repositories.Map
{
    public class MapPaymentAudit : IEntityTypeConfiguration<PaymentAudit>
    {
        public void Configure(EntityTypeBuilder<PaymentAudit> builder)
        {
            builder.ToTable("PaymentAudits");

            builder.HasKey(pa => pa.Id);

            builder.Property(pa => pa.Id)
                .IsRequired()
                .HasColumnName("Id");

            builder.Property(pa => pa.PaymentId)
                .IsRequired()
                .HasColumnName("PaymentId");

            builder.Property(pa => pa.Action)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName("Action");

            builder.Property(pa => pa.Description)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("Description");

            builder.Property(pa => pa.PreviousData)
                .HasColumnType("TEXT")
                .HasColumnName("PreviousData");

            builder.Property(pa => pa.NewData)
                .HasColumnType("TEXT")
                .HasColumnName("NewData");

            builder.Property(pa => pa.UserId)
                .IsRequired()
                .HasColumnName("UserId");

            builder.Property(pa => pa.ActionDate)
                .IsRequired()
                .HasColumnName("ActionDate");

            builder.Property(pa => pa.IpAddress)
                .HasMaxLength(45)
                .HasColumnName("IpAddress");

            builder.Property(pa => pa.UserAgent)
                .HasMaxLength(500)
                .HasColumnName("UserAgent");

            // Audit fields
            builder.Property(pa => pa.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(pa => pa.UpdatedAt)
                .HasColumnName("UpdatedAt");

            builder.Property(pa => pa.CreatedBy)
                .HasColumnName("CreatedBy");

            builder.Property(pa => pa.UpdatedBy)
                .HasColumnName("UpdatedBy");

            builder.Property(pa => pa.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsDeleted");

            builder.Property(pa => pa.DeletedAt)
                .HasColumnName("DeletedAt");

            builder.Property(pa => pa.DeletedBy)
                .HasColumnName("DeletedBy");

            // Relationships
            builder.HasOne(pa => pa.Payment)
                .WithMany()
                .HasForeignKey(pa => pa.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pa => pa.User)
                .WithMany()
                .HasForeignKey(pa => pa.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(pa => pa.PaymentId)
                .HasDatabaseName("IX_PaymentAudits_PaymentId");

            builder.HasIndex(pa => pa.UserId)
                .HasDatabaseName("IX_PaymentAudits_UserId");

            builder.HasIndex(pa => pa.Action)
                .HasDatabaseName("IX_PaymentAudits_Action");

            builder.HasIndex(pa => pa.ActionDate)
                .HasDatabaseName("IX_PaymentAudits_ActionDate");

            // Query filter for soft delete
            builder.HasQueryFilter(pa => !pa.IsDeleted);
        }
    }
}