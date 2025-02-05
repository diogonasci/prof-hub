using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.PrivateLesson;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
internal sealed class PrivateLessonConfiguration : IEntityTypeConfiguration<PrivateClass>
{
    public void Configure(EntityTypeBuilder<PrivateClass> builder)
    {
        builder.ToTable("private_lessons");

        builder.HasKey(pl => pl.Id);

        builder.Property(pl => pl.TeacherId).IsRequired().HasColumnName("teacher_id");
        builder.Property(pl => pl.StudentId).IsRequired().HasColumnName("student_id");

        builder.Property(pl => pl.Price)
            .HasConversion(
                price => price.Value,
                value => Price.Create(value).Value)
            .IsRequired()
            .HasColumnName("price");

        builder.Property(pl => pl.StartTime).IsRequired().HasColumnName("start_time");
        builder.Property(pl => pl.EndTime).IsRequired().HasColumnName("end_time");

        builder.Property(pl => pl.Status)
            .IsRequired()
            .HasColumnName("status");

        builder.Property(pl => pl.Created).IsRequired().HasColumnName("created");
        builder.Property(pl => pl.CreatedBy).HasMaxLength(50).HasColumnName("created_by");
        builder.Property(pl => pl.LastModified).IsRequired(false).HasColumnName("last_modified");
        builder.Property(pl => pl.LastModifiedBy).HasMaxLength(50).HasColumnName("last_modified_by");
    }
}
