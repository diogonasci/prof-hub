using Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial;
public class ClassMaterial : AuditableEntity
{
    public ClassMaterialId Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Uri FileUrl { get; private set; }
    public MaterialType Type { get; private set; }
    public bool IsAvailableBeforeClass { get; private set; }

    private ClassMaterial()
    {
    }

    public static Result<ClassMaterial> Create(
            string title,
            string description,
            Uri fileUrl,
            MaterialType type,
            bool isAvailableBeforeClass)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(title))
            errors.Add(new ValidationError("Título é obrigatório."));

        if (fileUrl == null)
            errors.Add(new ValidationError("URL do arquivo é obrigatória."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var classMaterial = new ClassMaterial
        {
            Title = title,
            Description = description,
            FileUrl = fileUrl,
            Type = type,
            IsAvailableBeforeClass = isAvailableBeforeClass
        };

        return classMaterial;
    }
}
