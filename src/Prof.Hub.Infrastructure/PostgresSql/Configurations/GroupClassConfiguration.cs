using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback;
using Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback.ValueObjects;
using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.GroupClass;
using Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.Domain.Enums;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;

internal sealed class GroupClassConfiguration : IEntityTypeConfiguration<GroupClass>
{
    public void Configure(EntityTypeBuilder<GroupClass> builder)
    {
        // Tabela e chave primária
        builder.ToTable("GroupClasses");
        builder.HasKey(g => g.Id);

        // Conversão do Value Object GroupClassId
        builder.Property(g => g.Id)
            .HasConversion(
                id => id.Value,
                value => new GroupClassId(value));

        // TeacherId (conversão)
        builder.Property(g => g.TeacherId)
            .HasConversion(
                id => id.Value,
                value => new TeacherId(value));

        // Propriedades básicas
        builder.Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(g => g.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(g => g.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(g => g.ThumbnailUrl)
            .HasConversion(
                url => url.ToString(),
                value => new Uri(value));

        builder.Property(g => g.AllowLateEnrollment);
        builder.Property(g => g.EnrollmentDeadline);

        // Configuração de status usando propriedade shadow
        builder.Property<ClassStatus>("_status")
            .HasColumnName("Status")
            .HasConversion<string>();

        // ParticipantLimit (entidade owned)
        builder.OwnsOne(g => g.ParticipantLimit, limit =>
        {
            limit.Property(l => l.Value)
                .HasColumnName("ParticipantLimit")
                .IsRequired();
        });

        // Subject (entidade owned)
        builder.OwnsOne(g => g.Subject, subject =>
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

        // Schedule (entidade owned)
        builder.OwnsOne(g => g.Schedule, schedule =>
        {
            schedule.Property(s => s.StartDate)
                .HasColumnName("StartDate");

            schedule.Property(s => s.Duration)
                .HasColumnName("Duration");
        });

        // Price (entidade owned)
        builder.OwnsOne(g => g.Price, price =>
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

        // Participants (coleção privada)
        builder.OwnsMany<StudentId>("_participants", participant =>
        {
            participant.ToTable("GroupClassParticipants");
            participant.WithOwner().HasForeignKey("GroupClassId");
            participant.Property<int>("Id").ValueGeneratedOnAdd();
            participant.HasKey("Id");

            participant.Property(p => p.Value)
                .HasColumnName("StudentId");
        });

        // WaitingList (coleção privada)
        builder.OwnsMany<StudentId>("_waitingList", waiting =>
        {
            waiting.ToTable("GroupClassWaitingList");
            waiting.WithOwner().HasForeignKey("GroupClassId");
            waiting.Property<int>("Id").ValueGeneratedOnAdd();
            waiting.HasKey("Id");

            waiting.Property(w => w.Value)
                .HasColumnName("StudentId");
        });

        // PresenceList (coleção privada)
        builder.OwnsMany<StudentPresence>("_presenceList", presence =>
        {
            presence.ToTable("GroupClassPresence");
            presence.WithOwner().HasForeignKey("GroupClassId");
            presence.Property<int>("Id").ValueGeneratedOnAdd();
            presence.HasKey("Id");

            presence.Property(p => p.StudentId)
                .HasConversion(
                    id => id.Value,
                    value => new StudentId(value))
                .HasColumnName("StudentId");

            presence.Property(p => p.PresenceTime);
        });

        // Discounts (coleção privada)
        builder.OwnsMany<GroupDiscount>("_discounts", discount =>
        {
            discount.ToTable("GroupClassDiscounts");
            discount.WithOwner().HasForeignKey("GroupClassId");
            discount.Property<int>("Id").ValueGeneratedOnAdd();
            discount.HasKey("Id");

            discount.Property(d => d.MinParticipants);
            discount.Property(d => d.PercentageDiscount)
                .HasPrecision(5, 2);
        });

        // LimitChanges (coleção privada)
        builder.OwnsMany<ParticipantLimitChange>("_limitChanges", change =>
        {
            change.ToTable("GroupClassLimitChanges");
            change.WithOwner().HasForeignKey("GroupClassId");
            change.Property<int>("Id").ValueGeneratedOnAdd();
            change.HasKey("Id");

            change.OwnsOne(c => c.Limit, limit =>
            {
                limit.Property(l => l.Value)
                    .HasColumnName("NewLimit");
            });
            change.Property(c => c.ChangedAt);
            change.Property(c => c.Reason)
                .HasMaxLength(500);
        });

        // Requirements (coleção privada)
        builder.OwnsMany<ClassRequirement>("_requirements", req =>
        {
            req.ToTable("GroupClassRequirements");
            req.WithOwner().HasForeignKey("GroupClassId");
            req.Property<int>("Id").ValueGeneratedOnAdd();
            req.HasKey("Id");

            req.Property(r => r.Description)
                .HasMaxLength(500);
            req.Property(r => r.IsMandatory);
        });

        // Shares (coleção privada)
        builder.OwnsMany<SocialShare>("_shares", share =>
        {
            share.ToTable("GroupClassShares");
            share.WithOwner().HasForeignKey("GroupClassId");
            share.Property<int>("Id").ValueGeneratedOnAdd();
            share.HasKey("Id");

            share.Property(s => s.SharedBy)
                .HasConversion(
                    id => id.Value,
                    value => new StudentId(value))
                .HasColumnName("StudentId");

            share.Property(s => s.Network)
                .HasConversion<string>();
            share.Property(s => s.SharedAt);
            share.Property(s => s.ShareUrl)
                .HasConversion(
                    url => url.ToString(),
                    value => new Uri(value));
        });

        // StatusHistory (herdado de ClassBase)
        builder.OwnsMany<StatusChange>("_statusHistory", status =>
        {
            status.ToTable("GroupClassStatusHistory");
            status.WithOwner().HasForeignKey("GroupClassId");
            status.Property<int>("Id").ValueGeneratedOnAdd();
            status.HasKey("Id");

            status.Property(s => s.PreviousStatus)
                .HasConversion<string>();
            status.Property(s => s.NewStatus)
                .HasConversion<string>();
            status.Property(s => s.ChangedAt);
        });

        // ClassFeedback (herdado de ClassBase)
        builder.OwnsMany<ClassFeedback>("_feedbacks", feedback =>
        {
            feedback.ToTable("GroupClassFeedbacks");
            feedback.WithOwner().HasForeignKey("GroupClassId");
            feedback.HasKey(f => f.Id);
            feedback.Property(f => f.Id)
                .HasConversion(
                    id => id.Value,
                    value => new ClassFeedbackId(value));
            feedback.HasKey("Id");

            feedback.OwnsOne(f => f.OverallRating);
            feedback.OwnsOne(f => f.TeachingRating);
            feedback.OwnsOne(f => f.MaterialsRating);
            feedback.OwnsOne(f => f.TechnicalRating);

            feedback.Property(f => f.TeacherComment)
                .HasMaxLength(1000);
            feedback.Property(f => f.TechnicalComment)
                .HasMaxLength(1000);
            feedback.Property(f => f.IsAnonymous);
            feedback.Property(f => f.HadTechnicalIssues);
        });

        // ClassIssues (herdado de ClassBase)
        builder.OwnsMany<ClassIssue>("_issues", issue =>
        {
            issue.ToTable("GroupClassIssues");
            issue.WithOwner().HasForeignKey("GroupClassId");
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
        });

        // Attendance (herdado de ClassBase)
        builder.OwnsMany<Attendance>("_attendance", attendance =>
        {
            attendance.ToTable("GroupClassAttendance");
            attendance.WithOwner().HasForeignKey("GroupClassId");
            attendance.Property<int>("Id").ValueGeneratedOnAdd();
            attendance.HasKey("Id");

            attendance.Property(a => a.ParticipantId);
            attendance.Property(a => a.Type)
                .HasConversion<string>();
            attendance.Property(a => a.JoinTime);
        });

        // Materials (herdado de ClassBase)
        builder.HasMany("_materials")
            .WithOne()
            .HasForeignKey("GroupClassId")
            .OnDelete(DeleteBehavior.Restrict);

        // Propriedades de auditoria
        builder.Property(g => g.Created);
        builder.Property(g => g.CreatedBy)
            .HasMaxLength(100);
        builder.Property(g => g.LastModified);
        builder.Property(g => g.LastModifiedBy)
            .HasMaxLength(100);

        // Índices
        builder.HasIndex(g => g.TeacherId);
        builder.HasIndex(g => g.Slug).IsUnique();
        builder.HasIndex("_status").HasDatabaseName("IX_GroupClasses_Status");
        builder.HasIndex(g => g.StartedAt);
        builder.HasIndex(g => g.EnrollmentDeadline);
    }
}
