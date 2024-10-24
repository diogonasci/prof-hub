using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.Common;
using Prof.Hub.Domain.Aggregates.Teacher;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
internal sealed class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("teachers");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value).Value)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        builder.Property(t => t.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value).Value)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("email");

        builder.Property(t => t.PhoneNumber)
            .HasConversion(
                phone => phone.Value,
                value => PhoneNumber.Create(value).Value)
            .IsRequired()
            .HasColumnName("phone_number");

        builder.OwnsOne(t => t.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("street").IsRequired().HasMaxLength(150);
            address.Property(a => a.City).HasColumnName("city").IsRequired().HasMaxLength(50);
            address.Property(a => a.State).HasColumnName("state").IsRequired().HasMaxLength(20);
            address.Property(a => a.PostalCode).HasColumnName("postal_code").IsRequired();
        });

        builder.Property(t => t.HourlyRate)
            .HasConversion(
                rate => rate.Value,
                value => HourlyRate.Create(value).Value)
            .IsRequired()
            .HasColumnName("hourly_rate");

        builder.HasMany(t => t.PrivateLessons)
            .WithOne()
            .HasForeignKey("TeacherId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.GroupLessons)
            .WithOne()
            .HasForeignKey("TeacherId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.Created).IsRequired().HasColumnName("created");
        builder.Property(t => t.CreatedBy).HasMaxLength(50).HasColumnName("created_by");
        builder.Property(t => t.LastModified).IsRequired(false).HasColumnName("last_modified");
        builder.Property(t => t.LastModifiedBy).HasMaxLength(50).HasColumnName("last_modified_by");
    }
}
