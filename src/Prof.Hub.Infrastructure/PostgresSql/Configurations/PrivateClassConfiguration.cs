using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback;
using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.PrivateClass;
using Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.Domain.Enums;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
internal sealed class PrivateClassConfiguration : IEntityTypeConfiguration<PrivateClass>
{
    public void Configure(EntityTypeBuilder<PrivateClass> builder)
    {
        // Tabela e chave primária
        builder.ToTable("PrivateClasses");
        builder.HasKey(p => p.Id);

        // Conversão do Value Object PrivateClassId
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => new PrivateClassId(value));

        // Status configuration using shadow property
        builder.Property<ClassStatus>("_status")
            .HasColumnName("Status")
            .HasConversion<string>();

        // TeacherId (conversão)
        builder.Property(p => p.TeacherId)
            .HasConversion(
                id => id.Value,
                value => new TeacherId(value));

        // StudentId (conversão)
        builder.Property(p => p.StudentId)
            .HasConversion(
                id => id.Value,
                value => new StudentId(value));

        // Subject (owned entity)
        builder.OwnsOne(p => p.Subject, subject =>
        {
            subject.Property(s => s.Name)
                .HasColumnName("SubjectName")
                .HasMaxLength(100);

            subject.Property(s => s.Description)
                .HasColumnName("SubjectDescription")
                .HasMaxLength(500);

            subject.Property(s => s.Area)
                .HasColumnName("SubjectArea")
                .HasConversion<string>();

            subject.Property(s => s.Level)
                .HasColumnName("EducationLevel")
                .HasConversion<string>();

            subject.Property<string[]>("Topics")
                .HasColumnName("SubjectTopics")
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
        });

        // Schedule (owned entity)
        builder.OwnsOne(p => p.Schedule, schedule =>
        {
            schedule.Property(s => s.StartDate)
                .HasColumnName("StartDate");

            schedule.Property(s => s.Duration)
                .HasColumnName("Duration");
        });

        // Price (owned entity)
        builder.OwnsOne(p => p.Price, price =>
        {
            price.OwnsOne(p => p.Value, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("Price")
                    .HasPrecision(18, 2);

                money.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .HasDefaultValue("BRL");
            });
        });

        // MeetingUrl
        builder.Property(p => p.MeetingUrl)
            .HasConversion(
                url => url.ToString(),
                value => new Uri(value));

        // Flags
        builder.Property(p => p.StudentPresent)
            .HasDefaultValue(false);

        builder.Property(p => p.TeacherPresent)
            .HasDefaultValue(false);

        builder.Property(p => p.StudentJoinedAt);
        builder.Property(p => p.TeacherJoinedAt);
        builder.Property(p => p.StartedAt);
        builder.Property(p => p.CompletedAt);
        builder.Property(p => p.EffectiveDuration);

        // Notes (owned collection)
        builder.OwnsMany<ClassNote>("_notes", note =>
        {
            note.ToTable("PrivateClassNotes");
            note.WithOwner().HasForeignKey("PrivateClassId");
            note.Property<int>("Id").ValueGeneratedOnAdd();
            note.HasKey("Id");

            note.Property(n => n.Content)
                .HasMaxLength(1000);

            note.Property(n => n.CreatedAt);

            // Índices
            note.HasIndex(n => n.CreatedAt);
        });

        // ScheduleChanges (owned collection)
        builder.OwnsMany<ScheduleChange>("_scheduleChanges", change =>
        {
            change.ToTable("PrivateClassScheduleChanges");
            change.WithOwner().HasForeignKey("PrivateClassId");
            change.Property<int>("Id").ValueGeneratedOnAdd();
            change.HasKey("Id");

            change.Property(c => c.ClassId)
                .HasConversion(
                    id => id.Value,
                    value => new PrivateClassId(value));

            change.Property(c => c.OldStartDate);
            change.Property(c => c.OldDuration);
            change.Property(c => c.NewStartDate);
            change.Property(c => c.NewDuration);
            change.Property(c => c.ChangedAt);

            // Índices
            change.HasIndex(c => c.ChangedAt);
        });

        // StatusHistory (inherited from ClassBase)
        builder.OwnsMany<StatusChange>("_statusHistory", status =>
        {
            status.ToTable("PrivateClassStatusHistory");
            status.WithOwner().HasForeignKey("PrivateClassId");
            status.Property<int>("Id").ValueGeneratedOnAdd();
            status.HasKey("Id");

            status.Property(s => s.PreviousStatus)
                .HasConversion<string>();
            status.Property(s => s.NewStatus)
                .HasConversion<string>();
            status.Property(s => s.ChangedAt);

            // Índices
            status.HasIndex(s => s.ChangedAt);
        });

        // ClassFeedback (inherited from ClassBase)
        builder.OwnsMany<ClassFeedback>("_feedbacks", feedback =>
        {
            feedback.ToTable("PrivateClassFeedbacks");
            feedback.WithOwner().HasForeignKey("PrivateClassId");
            feedback.Property<string>("Id")
                .HasMaxLength(36);
            feedback.HasKey("Id");

            feedback.OwnsOne(f => f.OverallRating, rating =>
            {
                rating.Property(r => r.Value)
                    .HasColumnName("OverallRating");
            });

            feedback.OwnsOne(f => f.TeachingRating, rating =>
            {
                rating.Property(r => r.Value)
                    .HasColumnName("TeachingRating");
            });

            feedback.OwnsOne(f => f.MaterialsRating, rating =>
            {
                rating.Property(r => r.Value)
                    .HasColumnName("MaterialsRating");
            });

            feedback.OwnsOne(f => f.TechnicalRating, rating =>
            {
                rating.Property(r => r.Value)
                    .HasColumnName("TechnicalRating");
            });

            feedback.Property(f => f.TeacherComment)
                .HasMaxLength(1000);
            feedback.Property(f => f.TechnicalComment)
                .HasMaxLength(1000);
            feedback.Property(f => f.IsAnonymous);
            feedback.Property(f => f.HadTechnicalIssues);

            // Índices
            feedback.HasIndex(f => f.IsAnonymous);
            feedback.HasIndex(f => f.HadTechnicalIssues);
        });

        // ClassIssues (inherited from ClassBase)
        builder.OwnsMany<ClassIssue>("_issues", issue =>
        {
            issue.ToTable("PrivateClassIssues");
            issue.WithOwner().HasForeignKey("PrivateClassId");
            issue.Property<int>("Id").ValueGeneratedOnAdd();
            issue.HasKey("Id");

            issue.Property(i => i.Description)
                .HasMaxLength(500);
            issue.Property(i => i.Type)
                .HasConversion<string>();
            issue.Property(i => i.ReportedAt);
            issue.Property(i => i.IsResolved);
            issue.Property(i => i.ResolvedAt);
            issue.Property(i => i.ResolutionNotes)
                .HasMaxLength(500);

            // Índices
            issue.HasIndex(i => i.Type);
            issue.HasIndex(i => i.ReportedAt);
            issue.HasIndex(i => i.IsResolved);
        });

        // Attendance (inherited from ClassBase)
        builder.OwnsMany<Attendance>("_attendance", attendance =>
        {
            attendance.ToTable("PrivateClassAttendance");
            attendance.WithOwner().HasForeignKey("PrivateClassId");
            attendance.Property<int>("Id").ValueGeneratedOnAdd();
            attendance.HasKey("Id");

            attendance.Property(a => a.ParticipantId);
            attendance.Property(a => a.Type)
                .HasConversion<string>();
            attendance.Property(a => a.JoinTime);

            // Índices
            attendance.HasIndex(a => a.ParticipantId);
            attendance.HasIndex(a => a.JoinTime);
        });

        // Materials (inherited from ClassBase)
        builder.HasMany("_materials")
            .WithOne()
            .HasForeignKey("PrivateClassId")
            .OnDelete(DeleteBehavior.Restrict);

        // Audit properties
        builder.Property(p => p.Created);
        builder.Property(p => p.CreatedBy)
            .HasMaxLength(100);
        builder.Property(p => p.LastModified);
        builder.Property(p => p.LastModifiedBy)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(p => p.TeacherId);
        builder.HasIndex(p => p.StudentId);
        builder.HasIndex("_status").HasDatabaseName("IX_PrivateClasses_Status");
        builder.HasIndex(p => p.StartedAt);
        builder.HasIndex(p => p.CompletedAt);
    }
}
