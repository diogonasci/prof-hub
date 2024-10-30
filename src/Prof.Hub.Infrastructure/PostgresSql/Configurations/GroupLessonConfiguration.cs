using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.GroupLesson;
using Prof.Hub.Domain.Aggregates.Student;
using Prof.Hub.Domain.Aggregates.PrivateLesson.ValueObjects;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
internal sealed class GroupLessonConfiguration : IEntityTypeConfiguration<GroupLesson>
{
    public void Configure(EntityTypeBuilder<GroupLesson> builder)
    {
        builder.ToTable("group_lessons");

        builder.HasKey(gl => gl.Id);

        builder.Property(gl => gl.TeacherId)
            .IsRequired()
            .HasColumnName("teacher_id");

        builder.Property(gl => gl.Price)
            .HasConversion(
                price => price.Value,
                value => Price.Create(value).Value)
            .IsRequired()
            .HasColumnName("price");

        builder.Property(gl => gl.StartTime)
            .IsRequired()
            .HasColumnName("start_time");

        builder.Property(gl => gl.EndTime)
            .IsRequired()
            .HasColumnName("end_time");

        builder.Property(gl => gl.Status)
            .IsRequired()
            .HasColumnName("status");

        builder.Property(gl => gl.Created)
            .IsRequired()
            .HasColumnName("created");

        builder.Property(gl => gl.CreatedBy)
            .HasMaxLength(50)
            .HasColumnName("created_by");

        builder.Property(gl => gl.LastModified)
            .IsRequired(false)
            .HasColumnName("last_modified");

        builder.Property(gl => gl.LastModifiedBy)
            .HasMaxLength(50)
            .HasColumnName("last_modified_by");

        // Configuração do relacionamento many-to-many
        builder.HasMany(gl => gl.Students)
            .WithMany(s => s.GroupLessons)
            .UsingEntity<Dictionary<string, object>>(
                "GroupLessonStudent",
                j => j
                    .HasOne<Student>()
                    .WithMany()
                    .HasForeignKey("StudentId")
                    .HasConstraintName("FK_GroupLessonStudent_Student")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<GroupLesson>()
                    .WithMany()
                    .HasForeignKey("GroupLessonId")
                    .HasConstraintName("FK_GroupLessonStudent_GroupLesson")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.ToTable("group_lesson_students");
                    j.HasKey("GroupLessonId", "StudentId");
                });
    }
}
