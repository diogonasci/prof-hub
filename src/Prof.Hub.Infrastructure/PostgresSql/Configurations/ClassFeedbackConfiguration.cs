using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback;
using Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback.ValueObjects;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
internal sealed class ClassFeedbackConfiguration : IEntityTypeConfiguration<ClassFeedback>
{
    public void Configure(EntityTypeBuilder<ClassFeedback> builder)
    {
        // Tabela, chave primária e constraints
        builder.ToTable(tb =>
        {
            tb.HasCheckConstraint(
                "CK_ClassFeedback_Comments_Length",
                """
                (\"TeacherComment\" IS NULL OR LENGTH(\"TeacherComment\") <= 1000) AND 
                (\"TechnicalComment\" IS NULL OR LENGTH(\"TechnicalComment\") <= 1000)
                """);

            tb.HasCheckConstraint(
                "CK_ClassFeedback_Ratings_Range",
                """
                \"OverallRating\" BETWEEN 1 AND 5 AND
                \"TeachingRating\" BETWEEN 1 AND 5 AND
                \"MaterialsRating\" BETWEEN 1 AND 5 AND
                \"TechnicalRating\" BETWEEN 1 AND 5
                """);
        });

        builder.HasKey(f => f.Id);

        // Conversão do Value Object ClassFeedbackId
        builder.Property(f => f.Id)
            .HasConversion(
                id => id.Value,
                value => new ClassFeedbackId(value));

        // OverallRating (owned)
        builder.OwnsOne(f => f.OverallRating, rating =>
        {
            rating.Property(r => r.Value)
                .HasColumnName("OverallRating")
                .IsRequired();
        });

        // TeachingRating (owned)
        builder.OwnsOne(f => f.TeachingRating, rating =>
        {
            rating.Property(r => r.Value)
                .HasColumnName("TeachingRating")
                .IsRequired();
        });

        // MaterialsRating (owned)
        builder.OwnsOne(f => f.MaterialsRating, rating =>
        {
            rating.Property(r => r.Value)
                .HasColumnName("MaterialsRating")
                .IsRequired();
        });

        // TechnicalRating (owned)
        builder.OwnsOne(f => f.TechnicalRating, rating =>
        {
            rating.Property(r => r.Value)
                .HasColumnName("TechnicalRating")
                .IsRequired();
        });

        // Comentários
        builder.Property(f => f.TeacherComment)
            .HasMaxLength(1000);

        builder.Property(f => f.TechnicalComment)
            .HasMaxLength(1000);

        // Flags
        builder.Property(f => f.IsAnonymous)
            .HasDefaultValue(false);

        builder.Property(f => f.HadTechnicalIssues)
            .HasDefaultValue(false);

        // Propriedades de auditoria
        builder.Property(f => f.Created);
        builder.Property(f => f.CreatedBy)
            .HasMaxLength(100);
        builder.Property(f => f.LastModified);
        builder.Property(f => f.LastModifiedBy)
            .HasMaxLength(100);

        // Índices
        builder.HasIndex(f => f.Created);
        builder.HasIndex(f => f.IsAnonymous);
        builder.HasIndex(f => f.HadTechnicalIssues);

        // Índice composto para análise de avaliações
        builder.HasIndex(
            "OverallRating",
            "TeachingRating",
            "MaterialsRating",
            "TechnicalRating")
            .HasDatabaseName("IX_ClassFeedback_AllRatings");
    }
}

// Configuração complementar para ClassFeedbackSummary
internal sealed class ClassFeedbackSummaryConfiguration : IEntityTypeConfiguration<ClassFeedbackSummary>
{
    public void Configure(EntityTypeBuilder<ClassFeedbackSummary> builder)
    {
        builder.ToTable("ClassFeedbackSummaries");

        builder.Property(s => s.AverageOverallRating)
            .HasPrecision(3, 2)
            .IsRequired();

        builder.Property(s => s.AverageTeachingRating)
            .HasPrecision(3, 2)
            .IsRequired();

        builder.Property(s => s.AverageMaterialsRating)
            .HasPrecision(3, 2)
            .IsRequired();

        builder.Property(s => s.AverageTechnicalRating)
            .HasPrecision(3, 2)
            .IsRequired();

        builder.Property(s => s.TotalFeedbacks)
            .IsRequired();

        builder.Property(s => s.TechnicalIssuesCount)
            .IsRequired();
    }
}
