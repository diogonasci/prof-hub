using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial;
using Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial.ValueObjects;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;

internal sealed class ClassMaterialConfiguration : IEntityTypeConfiguration<ClassMaterial>
{
    public void Configure(EntityTypeBuilder<ClassMaterial> builder)
    {
        builder.HasKey(m => m.Id);

        // Conversão do Value Object ClassMaterialId
        builder.Property(m => m.Id)
            .HasConversion(
                id => id.Value,
                value => new ClassMaterialId(value));

        // Propriedades básicas
        builder.Property(m => m.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(m => m.FileUrl)
            .HasConversion(
                url => url.ToString(),
                value => new Uri(value))
            .IsRequired();

        builder.Property(m => m.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(m => m.IsAvailableBeforeClass)
            .HasDefaultValue(false);

        builder.Property(m => m.FileSizeInBytes)
            .IsRequired();

        builder.Property(m => m.FileFormat)
            .HasMaxLength(50)
            .IsRequired();

        // Versions (coleção owned)
        builder.OwnsMany<MaterialVersion>("_versions", version =>
        {
            version.ToTable("ClassMaterialVersions");
            version.WithOwner().HasForeignKey("ClassMaterialId");

            version.Property<int>("Id").ValueGeneratedOnAdd();
            version.HasKey("Id");

            version.Property(v => v.MaterialId)
                .HasConversion(
                    id => id.Value,
                    value => new ClassMaterialId(value))
                .IsRequired();

            version.Property(v => v.FileUrl)
                .HasConversion(
                    url => url.ToString(),
                    value => new Uri(value))
                .IsRequired();

            version.Property(v => v.FileSizeInBytes)
                .IsRequired();

            version.Property(v => v.FileFormat)
                .HasMaxLength(50)
                .IsRequired();

            version.Property(v => v.CreatedAt)
                .IsRequired();

            // Índices
            version.HasIndex(v => v.CreatedAt);
        });

        // Accesses (coleção owned)
        builder.OwnsMany<MaterialAccess>("_accesses", access =>
        {
            access.ToTable("ClassMaterialAccesses");
            access.WithOwner().HasForeignKey("ClassMaterialId");

            access.Property<int>("Id").ValueGeneratedOnAdd();
            access.HasKey("Id");

            access.Property(a => a.MaterialId)
                .HasConversion(
                    id => id.Value,
                    value => new ClassMaterialId(value))
                .IsRequired();

            access.Property(a => a.StudentId)
                .HasMaxLength(36)
                .IsRequired();

            access.Property(a => a.AccessTime)
                .IsRequired();

            // Índices
            access.HasIndex(a => a.StudentId);
            access.HasIndex(a => a.AccessTime);
        });

        // Propriedades de auditoria
        builder.Property(m => m.Created);
        builder.Property(m => m.CreatedBy)
            .HasMaxLength(100);
        builder.Property(m => m.LastModified);
        builder.Property(m => m.LastModifiedBy)
            .HasMaxLength(100);

        // Índices
        builder.HasIndex(m => m.Type);
        builder.HasIndex(m => m.IsAvailableBeforeClass);
        builder.HasIndex(m => m.Created);
        builder.HasIndex(m => m.FileFormat);
    }
}
