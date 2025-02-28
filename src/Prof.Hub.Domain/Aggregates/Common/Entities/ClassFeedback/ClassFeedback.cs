using Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback.ValueObjects;
using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback;
public class ClassFeedback : AuditableEntity
{
    private const int MAX_COMMENT_LENGTH = 1000;
    private const int MIN_RATING = 1;
    private const int MAX_RATING = 5;

    public ClassFeedbackId Id { get; private set; }
    public Rating OverallRating { get; private set; }
    public Rating TeachingRating { get; private set; }
    public Rating MaterialsRating { get; private set; }
    public Rating TechnicalRating { get; private set; }
    public string? TeacherComment { get; private set; }
    public string? TechnicalComment { get; private set; }
    public bool IsAnonymous { get; private set; }
    public bool HadTechnicalIssues { get; private set; }

    private ClassFeedback() { }

    public static Result<ClassFeedback> Create(
        int overallRating,
        int teachingRating,
        int materialsRating,
        int technicalRating,
        string? teacherComment,
        string? technicalComment,
        bool isAnonymous,
        bool hadTechnicalIssues)
    {
        var errors = new List<ValidationError>();

        var overallRatingResult = Rating.Create(overallRating);
        var teachingRatingResult = Rating.Create(teachingRating);
        var materialsRatingResult = Rating.Create(materialsRating);
        var technicalRatingResult = Rating.Create(technicalRating);

        if (!overallRatingResult.IsSuccess)
            errors.AddRange(overallRatingResult.ValidationErrors);

        if (!teachingRatingResult.IsSuccess)
            errors.AddRange(teachingRatingResult.ValidationErrors);

        if (!materialsRatingResult.IsSuccess)
            errors.AddRange(materialsRatingResult.ValidationErrors);

        if (!technicalRatingResult.IsSuccess)
            errors.AddRange(technicalRatingResult.ValidationErrors);

        if (teacherComment?.Length > MAX_COMMENT_LENGTH)
            errors.Add(new ValidationError($"Comentário para o professor não pode exceder {MAX_COMMENT_LENGTH} caracteres."));

        if (technicalComment?.Length > MAX_COMMENT_LENGTH)
            errors.Add(new ValidationError($"Comentário técnico não pode exceder {MAX_COMMENT_LENGTH} caracteres."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var feedback = new ClassFeedback
        {
            Id = ClassFeedbackId.Create(),
            OverallRating = overallRatingResult.Value,
            TeachingRating = teachingRatingResult.Value,
            MaterialsRating = materialsRatingResult.Value,
            TechnicalRating = technicalRatingResult.Value,
            TeacherComment = teacherComment,
            TechnicalComment = technicalComment,
            IsAnonymous = isAnonymous,
            HadTechnicalIssues = hadTechnicalIssues,
            Created = DateTime.UtcNow
        };

        return feedback;
    }

    public Result Update(
        string? teacherComment,
        string? technicalComment,
        bool? isAnonymous = null)
    {
        if (teacherComment?.Length > MAX_COMMENT_LENGTH)
            return Result.Invalid(new ValidationError($"Comentário para o professor não pode exceder {MAX_COMMENT_LENGTH} caracteres."));

        if (technicalComment?.Length > MAX_COMMENT_LENGTH)
            return Result.Invalid(new ValidationError($"Comentário técnico não pode exceder {MAX_COMMENT_LENGTH} caracteres."));

        TeacherComment = teacherComment;
        TechnicalComment = technicalComment;
        if (isAnonymous.HasValue)
            IsAnonymous = isAnonymous.Value;

        LastModified = DateTime.UtcNow;

        return Result.Success();
    }

    public decimal GetAverageRating()
    {
        var ratings = new[]
        {
            OverallRating.Value,
            TeachingRating.Value,
            MaterialsRating.Value,
            TechnicalRating.Value
        };

        return (decimal)ratings.Average();
    }

    public string GetFormattedTeacherComment()
    {
        if (string.IsNullOrWhiteSpace(TeacherComment))
            return string.Empty;

        return IsAnonymous ? "Comentário anônimo: " + TeacherComment : TeacherComment;
    }
}
