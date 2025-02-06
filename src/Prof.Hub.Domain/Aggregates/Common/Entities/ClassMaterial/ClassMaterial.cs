using Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial;
public class ClassMaterial : AuditableEntity
{
    private readonly List<MaterialVersion> _versions = [];
    private readonly List<MaterialAccess> _accesses = [];

    private const int MAX_TITLE_LENGTH = 100;
    private const int MAX_DESCRIPTION_LENGTH = 500;
    private const long MAX_FILE_SIZE = 100 * 1024 * 1024; // 100MB

    public ClassMaterialId Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Uri FileUrl { get; private set; }
    public MaterialType Type { get; private set; }
    public bool IsAvailableBeforeClass { get; private set; }
    public long FileSizeInBytes { get; private set; }
    public string FileFormat { get; private set; }

    public IReadOnlyList<MaterialVersion> Versions => _versions.AsReadOnly();
    public IReadOnlyList<MaterialAccess> Accesses => _accesses.AsReadOnly();

    private ClassMaterial() { }

    public static Result<ClassMaterial> Create(
        string title,
        string description,
        Uri fileUrl,
        MaterialType type,
        bool isAvailableBeforeClass,
        long fileSizeInBytes,
        string fileFormat)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(title))
            errors.Add(new ValidationError("Título é obrigatório."));

        if (title?.Length > MAX_TITLE_LENGTH)
            errors.Add(new ValidationError($"Título não pode exceder {MAX_TITLE_LENGTH} caracteres."));

        if (description?.Length > MAX_DESCRIPTION_LENGTH)
            errors.Add(new ValidationError($"Descrição não pode exceder {MAX_DESCRIPTION_LENGTH} caracteres."));

        if (!IsValidFileUrl(fileUrl))
            errors.Add(new ValidationError("URL do arquivo é inválida."));

        if (fileSizeInBytes > MAX_FILE_SIZE)
            errors.Add(new ValidationError($"Arquivo não pode exceder {MAX_FILE_SIZE / 1024 / 1024}MB."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var material = new ClassMaterial
        {
            Id = ClassMaterialId.Create(),
            Title = title,
            Description = description,
            FileUrl = fileUrl,
            Type = type,
            IsAvailableBeforeClass = isAvailableBeforeClass,
            FileSizeInBytes = fileSizeInBytes,
            FileFormat = fileFormat
        };

        material._versions.Add(new MaterialVersion(
            material.Id,
            fileUrl,
            fileSizeInBytes,
            fileFormat,
            DateTime.UtcNow));

        return material;
    }

    public Result Update(
        string title,
        string description,
        Uri? newFileUrl = null,
        long? newFileSizeInBytes = null,
        string? newFileFormat = null)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(title))
            errors.Add(new ValidationError("Título é obrigatório."));

        if (title.Length > MAX_TITLE_LENGTH)
            errors.Add(new ValidationError($"Título não pode exceder {MAX_TITLE_LENGTH} caracteres."));

        if (description?.Length > MAX_DESCRIPTION_LENGTH)
            errors.Add(new ValidationError($"Descrição não pode exceder {MAX_DESCRIPTION_LENGTH} caracteres."));

        if (newFileUrl != null && !IsValidFileUrl(newFileUrl))
            errors.Add(new ValidationError("Nova URL do arquivo é inválida."));

        if (newFileSizeInBytes > MAX_FILE_SIZE)
            errors.Add(new ValidationError($"Arquivo não pode exceder {MAX_FILE_SIZE / 1024 / 1024}MB."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        Title = title;
        Description = description;

        if (newFileUrl != null)
        {
            FileUrl = newFileUrl;
            FileSizeInBytes = newFileSizeInBytes ?? FileSizeInBytes;
            FileFormat = newFileFormat ?? FileFormat;

            _versions.Add(new MaterialVersion(
                Id,
                newFileUrl,
                FileSizeInBytes,
                FileFormat,
                DateTime.UtcNow));
        }

        return Result.Success();
    }

    public void SetAvailability(bool isAvailableBeforeClass)
    {
        IsAvailableBeforeClass = isAvailableBeforeClass;
    }

    public Result RegisterAccess(string studentId, DateTime accessTime)
    {
        _accesses.Add(new MaterialAccess(Id, studentId, accessTime));
        return Result.Success();
    }

    private static bool IsValidFileUrl(Uri? url)
    {
        if (url == null) return false;

        return url.Scheme == Uri.UriSchemeHttps &&
               !string.IsNullOrWhiteSpace(url.Host) &&
               !string.IsNullOrWhiteSpace(Path.GetExtension(url.LocalPath));
    }
}
