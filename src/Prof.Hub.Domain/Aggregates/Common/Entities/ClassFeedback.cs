using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common.Entities;
public class ClassFeedback : AuditableEntity
{
    public ClassFeedbackId Id { get; private set; }
    public Rating Rating { get; private set; }
    public string Comment { get; private set; }


    private ClassFeedback()
    {
    }

    public record ClassFeedbackId(string Value)
    {
        public static ClassFeedbackId Create() => new(Guid.NewGuid().ToString());
    }

    public static Result<ClassFeedback> Create(int rating, string comment, IDateTimeProvider dateTimeProvider)
    {
        var ratingResult = Rating.Create(rating);
        
        if (!ratingResult.IsSuccess)
        {
            var errors = new List<ValidationError>();

            if (ratingResult.ValidationErrors.Any()) errors.AddRange(ratingResult.ValidationErrors);

            return Result.Invalid(errors);
        }

        var classFeedback = new ClassFeedback
        {
            Id = ClassFeedbackId.Create(),
            Rating = rating,
            Comment = comment,
            Created = dateTimeProvider.UtcNow
        };

        return Result.Success(classFeedback);
    }
}
