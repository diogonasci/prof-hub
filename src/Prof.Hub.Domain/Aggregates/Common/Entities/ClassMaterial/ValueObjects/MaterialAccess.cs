namespace Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial.ValueObjects;
public record MaterialAccess(
    ClassMaterialId MaterialId,
    string StudentId,
    DateTime AccessTime);
