using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.Teacher;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
internal sealed class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("Teachers");

        builder.HasKey(t => t.Id);

        // Conversão do Value Object TeacherId
        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                value => new TeacherId(value));

        // Profile (owned entity)
        builder.OwnsOne(t => t.Profile, profile =>
        {
            profile.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            profile.Property(p => p.Bio)
                .IsRequired()
                .HasMaxLength(1000);

            profile.Property(p => p.AvatarUrl)
                .IsRequired()
                .HasMaxLength(500);
        });

        // Rating (owned entity)
        builder.OwnsOne(t => t.Rating, rating =>
        {
            rating.Property<decimal>("AverageScore")
                .HasColumnName("Rating")
                .HasPrecision(3, 2);

            rating.Property<int>("TotalRatings")
                .HasColumnName("TotalRatings");
        });

        // Status e última atividade
        builder.Property(t => t.Status)
            .HasConversion<string>();

        builder.Property(t => t.LastActiveAt);

        // TeacherAvailability (owned entity)
        builder.OwnsOne(t => t.Availability, availability =>
        {
            availability.OwnsMany(a => a.TimeSlots, timeSlot =>
            {
                timeSlot.ToTable("TeacherTimeSlots");
                timeSlot.WithOwner().HasForeignKey("TeacherId");
                timeSlot.Property<int>("Id").ValueGeneratedOnAdd();
                timeSlot.HasKey("Id");

                timeSlot.Property(ts => ts.DayOfWeek)
                    .HasConversion<string>();
                timeSlot.Property(ts => ts.StartTime);
                timeSlot.Property(ts => ts.EndTime);
            });
        });

        // Qualifications (owned collection)
        builder.OwnsMany(t => t.Qualifications, qualification =>
        {
            qualification.ToTable("TeacherQualifications");
            qualification.WithOwner().HasForeignKey("TeacherId");
            qualification.Property<int>("Id").ValueGeneratedOnAdd();
            qualification.HasKey("Id");

            qualification.Property(q => q.Title)
                .IsRequired()
                .HasMaxLength(200);

            qualification.Property(q => q.Institution)
                .IsRequired()
                .HasMaxLength(200);

            qualification.Property(q => q.ObtainedAt);
            qualification.Property(q => q.IsVerified)
                .HasDefaultValue(false);
        });

        // Specialties (owned collection)
        builder.OwnsMany(t => t.Specialties, specialty =>
        {
            specialty.ToTable("TeacherSpecialties");
            specialty.WithOwner().HasForeignKey("TeacherId");
            specialty.Property<int>("Id").ValueGeneratedOnAdd();
            specialty.HasKey("Id");

            specialty.Property(s => s.Area)
                .HasConversion<string>();
            specialty.Property(s => s.Description)
                .IsRequired()
                .HasMaxLength(200);
            specialty.Property(s => s.IsVerified)
                .HasDefaultValue(false);
        });

        // RateHistory (owned collection)
        builder.OwnsMany<HourlyRate>("_rateHistory", rate =>
        {
            rate.ToTable("TeacherRateHistory");
            rate.WithOwner().HasForeignKey("TeacherId");
            rate.Property<int>("Id").ValueGeneratedOnAdd();
            rate.HasKey("Id");

            rate.OwnsOne(r => r.Value, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("Rate")
                    .HasPrecision(18, 2);

                money.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .HasDefaultValue("BRL");
            });

            rate.Property(r => r.EffectiveFrom);
        });

        // Propriedades de auditoria
        builder.Property(t => t.Created);
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(100);
        builder.Property(t => t.LastModified);
        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(100);
    }
}
