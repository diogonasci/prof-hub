namespace Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial.ValueObjects;
public record MaterialVersion(
    ClassMaterialId MaterialId,
    Uri FileUrl,
    long FileSizeInBytes,
    string FileFormat,
    DateTime CreatedAt);
