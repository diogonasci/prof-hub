using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.Coupon.ValueObjects;
using Prof.Hub.Domain.Enums;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;

internal sealed class CouponRestrictionConfiguration : IEntityTypeConfiguration<CouponRestriction>
{
    public void Configure(EntityTypeBuilder<CouponRestriction> builder)
    {
        builder.ToTable("CouponRestrictions");

        builder.HasKey("Id");
        builder.Property<int>("Id").ValueGeneratedOnAdd();

        builder.Property(r => r.Type)
            .HasConversion<string>()
            .IsRequired();

        // Discriminador para os diferentes tipos de restrição
        builder.HasDiscriminator<string>("RestrictionType")
            .HasValue<MinimumOrderAmountRestriction>(CouponRestrictionType.MinimumOrderAmount.ToString())
            .HasValue<MaximumOrderAmountRestriction>(CouponRestrictionType.MaximumOrderAmount.ToString())
            .HasValue<SpecificSubjectRestriction>(CouponRestrictionType.SpecificSubject.ToString())
            .HasValue<SpecificTeacherRestriction>(CouponRestrictionType.SpecificTeacher.ToString())
            .HasValue<EducationLevelRestriction>(CouponRestrictionType.EducationLevel.ToString());

        // Propriedades específicas para cada tipo
        builder.Property<decimal?>("Amount")
            .HasColumnName("Amount")
            .HasPrecision(18, 2);

        builder.Property<string>("Currency")
            .HasColumnName("Currency")
            .HasMaxLength(3)
            .HasDefaultValue("BRL");

        builder.Property<SubjectArea?>("SubjectArea")
            .HasConversion<string>();

        builder.Property<string>("TeacherId")
            .HasMaxLength(36);

        builder.Property<EducationLevel?>("EducationLevel")
            .HasConversion<string>();

        builder.HasIndex(r => r.Type);
    }
}
