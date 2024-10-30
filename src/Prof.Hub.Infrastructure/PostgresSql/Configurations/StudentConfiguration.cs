using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.Common;
using Prof.Hub.Domain.Aggregates.Student;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
internal sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("students");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasConversion(
                name => name.Value,
                value => Name.Create(value).Value)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        builder.Property(s => s.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value).Value)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("email");

        builder.Property(s => s.PhoneNumber)
            .HasConversion(
                phone => phone.Value,
                value => PhoneNumber.Create(value).Value)
            .IsRequired()
            .HasColumnName("phone_number");

        builder.OwnsOne(s => s.Address, address =>
        {
            address.Property(a => a.Street)
                .HasColumnName("street")
                .IsRequired()
                .HasMaxLength(150);

            address.Property(a => a.City)
                .HasColumnName("city")
                .IsRequired()
                .HasMaxLength(50);

            address.Property(a => a.State)
                .HasColumnName("state")
                .IsRequired()
                .HasMaxLength(20);

            address.Property(a => a.PostalCode)
                .HasColumnName("postal_code")
                .IsRequired();
        });

        builder.OwnsOne(s => s.Parent, parent =>
        {
            parent.Property(p => p.Name)
                .HasConversion(
                    name => name.Value,
                    value => Name.Create(value).Value)
                .HasColumnName("parent_name")
                .IsRequired()
                .HasMaxLength(100);

            parent.Property(p => p.Email)
                .HasConversion(
                    email => email.Value,
                    value => Email.Create(value).Value)
                .HasColumnName("parent_email")
                .IsRequired()
                .HasMaxLength(100);

            parent.Property(p => p.PhoneNumber)
                .HasConversion(
                    phone => phone.Value,
                    value => PhoneNumber.Create(value).Value)
                .HasColumnName("parent_phone_number")
                .IsRequired();
        });

        builder.OwnsOne(s => s.ClassHours, classHours =>
        {
            classHours.Property(ch => ch.Value)
                .HasColumnName("class_hours")
                .IsRequired();
        });

        builder.HasMany(s => s.PrivateLessons)
            .WithOne()
            .HasForeignKey("StudentId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(s => s.Created)
            .IsRequired()
            .HasColumnName("created");

        builder.Property(s => s.CreatedBy)
            .HasMaxLength(50)
            .HasColumnName("created_by");

        builder.Property(s => s.LastModified)
            .IsRequired(false)
            .HasColumnName("last_modified");

        builder.Property(s => s.LastModifiedBy)
            .HasMaxLength(50)
            .HasColumnName("last_modified_by");
    }
}
