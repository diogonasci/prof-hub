namespace Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial.ValueObjects;
public sealed record ClassMaterialId(string Value)
{
    public static ClassMaterialId Create() => new(Guid.NewGuid().ToString());
}
