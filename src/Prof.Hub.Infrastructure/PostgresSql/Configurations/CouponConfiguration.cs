using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Coupon;
using Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
using Prof.Hub.Domain.Enums;

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

        // DiscountValue (owned)
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

        // UsageHistory (owned collection)
        builder.OwnsMany<CouponUsage>("_usageHistory", usage =>
        {
            usage.ToTable("CouponUsageHistory");
            usage.WithOwner().HasForeignKey("CouponId");
            usage.Property<int>("Id").ValueGeneratedOnAdd();
            usage.HasKey("Id");

            usage.Property(u => u.StudentId.Value)
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

            usage.HasIndex(u => u.StudentId);
            usage.HasIndex(u => u.UsedAt);
        });

        // Restrictions (owned collection)
        builder.OwnsMany<CouponRestriction>("_restrictions", restriction =>
        {
            restriction.ToTable("CouponRestrictions");
            restriction.WithOwner().HasForeignKey("CouponId");
            restriction.Property<int>("Id").ValueGeneratedOnAdd();
            restriction.HasKey("Id");

            restriction.Property(r => r.Type)
                .HasConversion<string>()
                .IsRequired();

            // Minimum Amount
            restriction.OwnsOne<Money>("MinimumAmount", money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("MinimumAmount")
                    .HasPrecision(18, 2);

                money.Property(m => m.Currency)
                    .HasColumnName("MinimumAmountCurrency")
                    .HasMaxLength(3)
                    .HasDefaultValue("BRL");
            });

            // Maximum Amount
            restriction.OwnsOne<Money>("MaximumAmount", money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("MaximumAmount")
                    .HasPrecision(18, 2);

                money.Property(m => m.Currency)
                    .HasColumnName("MaximumAmountCurrency")
                    .HasMaxLength(3)
                    .HasDefaultValue("BRL");
            });

            restriction.Property<SubjectArea?>("SubjectArea")
                .HasConversion<string>();

            restriction.Property<string>("TeacherId")
                .HasMaxLength(36);

            restriction.Property<EducationLevel?>("EducationLevel")
                .HasConversion<string>();

            restriction.HasIndex(r => r.Type);
        });

        // Check Constraints
        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint(
                "CK_Coupon_ValidDates",
                "\"ValidUntil\" > \"ValidFrom\"");

            tb.HasCheckConstraint(
                "CK_Coupon_MaxUses_Positive",
                "\"TotalMaxUses\" IS NULL OR \"TotalMaxUses\" > 0");

            tb.HasCheckConstraint(
                "CK_Coupon_MaxUsesPerStudent_Positive",
                "\"MaxUsesPerStudent\" IS NULL OR \"MaxUsesPerStudent\" > 0");

            tb.HasCheckConstraint(
                "CK_Coupon_DiscountValues",
                """
                CASE 
                    WHEN "Type" = 'PercentageDiscount' THEN 
                        "DiscountPercentage" > 0 AND "DiscountPercentage" <= 100
                    WHEN "Type" IN ('FixedDiscount', 'GiftCard') THEN 
                        "DiscountAmount" > 0
                    ELSE FALSE
                END
                """);
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
