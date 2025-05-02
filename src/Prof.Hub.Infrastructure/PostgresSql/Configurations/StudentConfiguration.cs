using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.GroupClass;
using Prof.Hub.Domain.Aggregates.PrivateClass;
using Prof.Hub.Domain.Aggregates.Student;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
internal sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        // Tabela e chave primária
        builder.ToTable("Students");
        builder.HasKey(s => s.Id);

        // Conversão do Value Object StudentId
        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => new StudentId(value));

        // Profile (owned entity) 
        builder.OwnsOne(s => s.Profile, profile =>
        {
            profile.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            profile.OwnsOne(p => p.Email, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("Email")
                    .IsRequired()
                    .HasMaxLength(256);
            });

            profile.OwnsOne(p => p.PhoneNumber, phone =>
            {
                phone.Property(p => p.Value)
                    .HasColumnName("PhoneNumber")
                    .HasMaxLength(20);
            });

            profile.Property(p => p.Grade)
                .HasColumnName("Grade")
                .HasConversion<string>();

            profile.Property(p => p.AvatarUrl)
                .HasConversion(
                    url => url != null ? url.ToString() : null,
                    value => value != null ? new Uri(value) : null);

            profile.OwnsOne(p => p.ReferralCode, code =>
            {
                code.Property(c => c.Value)
                    .HasColumnName("ReferralCode")
                    .HasMaxLength(20);
            });
        });

        // School (owned entity)
        builder.OwnsOne(s => s.School, school =>
        {
            school.Property(s => s.Name)
                .HasColumnName("SchoolName")
                .HasMaxLength(200);

            school.Property(s => s.City)
                .HasColumnName("SchoolCity")
                .HasMaxLength(100);

            school.Property(s => s.State)
                .HasColumnName("SchoolState")
                .HasMaxLength(2);

            school.Property(s => s.IsVerified)
                .HasColumnName("IsSchoolVerified")
                .HasDefaultValue(false);
        });

        // Balance (owned entity)
        builder.OwnsOne(s => s.Balance, balance =>
        {
            balance.OwnsOne(b => b.Amount, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("Balance")
                    .HasPrecision(18, 2);

                money.Property(m => m.Currency)
                    .HasColumnName("BalanceCurrency")
                    .HasMaxLength(3)
                    .HasDefaultValue("BRL");
            });
        });

        // Relacionamentos
        builder.HasMany<PrivateClass>("_privateClasses")
            .WithOne()
            .HasForeignKey("StudentId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany<GroupClass>("_groupClasses")
            .WithMany()
            .UsingEntity(join => join.ToTable("StudentGroupClasses"));

        // Collection de EnrollmentHistory
        builder.OwnsMany(s => s.EnrollmentHistory, history =>
        {
            history.ToTable("StudentEnrollmentHistory");
            history.WithOwner().HasForeignKey("StudentId");
            history.Property<Guid>("Id");
            history.HasKey("Id");

            history.Property(h => h.ClassId)
                .HasMaxLength(36);

            history.Property(h => h.ClassType)
                .HasMaxLength(20);

            history.Property(h => h.Status)
                .HasConversion<string>();

            history.Property(h => h.EnrolledAt)
                .IsRequired();

            history.Property(h => h.CompletedAt);

            history.OwnsOne(h => h.Rating, rating =>
            {
                rating.Property(r => r.Value)
                    .HasColumnName("Rating");
            });

            // Índices
            history.HasIndex(h => h.ClassId);
            history.HasIndex(h => h.EnrolledAt);
        });

        // Collection de TeacherFavorite
        builder.OwnsMany(s => s.FavoriteTeachers, favorite =>
        {
            favorite.ToTable("StudentFavoriteTeachers");
            favorite.WithOwner().HasForeignKey("StudentId");
            favorite.Property<int>("Id").ValueGeneratedOnAdd();
            favorite.HasKey("Id");

            favorite.Property(f => f.TeacherId)
                .HasConversion(
                    id => id.Value,
                    value => new TeacherId(value))
                .IsRequired();

            favorite.Property(f => f.AddedAt)
                .IsRequired();

            favorite.Property(f => f.Note)
                .HasMaxLength(500);

            // Índices
            favorite.HasIndex(f => f.TeacherId);
            favorite.HasIndex(f => f.AddedAt);

            // Restrição única para evitar duplicatas
            favorite.HasIndex(f => new { f.TeacherId, StudentId = EF.Property<string>(f, "StudentId") })
                .IsUnique();
        });

        // Propriedades de auditoria
        builder.Property(s => s.Created);
        builder.Property(s => s.CreatedBy)
            .HasMaxLength(100);
        builder.Property(s => s.LastModified);
        builder.Property(s => s.LastModifiedBy)
            .HasMaxLength(100);

        // Índices
        builder.HasIndex("Profile.Email").IsUnique();
        builder.HasIndex("Profile.PhoneNumber");
        builder.HasIndex("Profile.Grade");
    }
}
