using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.Coupon;
using Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;

internal sealed class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        // Tabela e chave primária
        builder.ToTable("Coupons");
        builder.HasKey(c => c.Id);

        // Conversão do Value Object CouponId
        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => new CouponId(value));

        // Propriedades básicas
        builder.Property(c => c.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Type)
            .HasConversion<string>()
            .IsRequired();

        // DiscountValue (complex property)
        builder.ComplexProperty<DiscountValue>("Value", value =>
        {
            value.Property<decimal>("Percentage")
                .HasColumnName("DiscountPercentage")
                .HasPrecision(5, 2);

            value.Property<decimal>("FixedAmount")
                .HasColumnName("DiscountAmount")
                .HasPrecision(18, 2);

            value.Property<string>("Currency")
                .HasColumnName("DiscountCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        // Período de validade
        builder.Property(c => c.ValidFrom)
            .IsRequired();
        builder.Property(c => c.ValidUntil)
            .IsRequired();

        // Limites de uso
        builder.Property(c => c.MaxUsesPerStudent);
        builder.Property(c => c.TotalMaxUses);

        // Status
        builder.Property(c => c.Status)
            .HasConversion<string>()
            .IsRequired();
        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        // UsageHistory (coleção owned)
        builder.OwnsMany<CouponUsage>("_usageHistory", usage =>
        {
            usage.ToTable("CouponUsageHistory");
            usage.WithOwner().HasForeignKey("CouponId");
            usage.Property<int>("Id").ValueGeneratedOnAdd();
            usage.HasKey("Id");

            usage.Property(u => u.StudentId)
                .HasConversion(
                    id => id.Value,
                    value => new StudentId(value))
                .HasColumnName("StudentId")
                .IsRequired();

            usage.OwnsOne(u => u.OrderAmount, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("OrderAmount")
                    .HasPrecision(18, 2)
                    .IsRequired();

                money.Property(m => m.Currency)
                    .HasColumnName("OrderCurrency")
                    .HasMaxLength(3)
                    .HasDefaultValue("BRL");
            });

            usage.Property(u => u.UsedAt)
                .IsRequired();

            // Índices
            usage.HasIndex(u => u.StudentId);
            usage.HasIndex(u => u.UsedAt);
        });

        // Propriedades de auditoria
        builder.Property(c => c.Created);
        builder.Property(c => c.CreatedBy)
            .HasMaxLength(100);
        builder.Property(c => c.LastModified);
        builder.Property(c => c.LastModifiedBy)
            .HasMaxLength(100);

        // Índices
        builder.HasIndex(c => c.Code)
            .IsUnique();
        builder.HasIndex(c => c.Type);
        builder.HasIndex(c => c.Status);
        builder.HasIndex(c => c.IsActive);
        builder.HasIndex(c => c.ValidFrom);
        builder.HasIndex(c => c.ValidUntil);
    }
}
