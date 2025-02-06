using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback.ValueObjects;
public record ClassFeedbackSummary
{
    public decimal AverageOverallRating { get; }
    public decimal AverageTeachingRating { get; }
    public decimal AverageMaterialsRating { get; }
    public decimal AverageTechnicalRating { get; }
    public int TotalFeedbacks { get; }
    public int TechnicalIssuesCount { get; }

    private ClassFeedbackSummary(
        decimal averageOverallRating,
        decimal averageTeachingRating,
        decimal averageMaterialsRating,
        decimal averageTechnicalRating,
        int totalFeedbacks,
        int technicalIssuesCount)
    {
        AverageOverallRating = averageOverallRating;
        AverageTeachingRating = averageTeachingRating;
        AverageMaterialsRating = averageMaterialsRating;
        AverageTechnicalRating = averageTechnicalRating;
        TotalFeedbacks = totalFeedbacks;
        TechnicalIssuesCount = technicalIssuesCount;
    }

    public static Result<ClassFeedbackSummary> Create(IEnumerable<ClassFeedback> feedbacks)
    {
        if (feedbacks == null || !feedbacks.Any())
            return Result.Invalid(new ValidationError("Nenhum feedback fornecido para análise."));

        var totalFeedbacks = feedbacks.Count();
        var summary = new ClassFeedbackSummary(
            averageOverallRating: (decimal)feedbacks.Average(f => f.OverallRating.Value),
            averageTeachingRating: (decimal)feedbacks.Average(f => f.TeachingRating.Value),
            averageMaterialsRating: (decimal)feedbacks.Average(f => f.MaterialsRating.Value),
            averageTechnicalRating: (decimal)feedbacks.Average(f => f.TechnicalRating.Value),
            totalFeedbacks: totalFeedbacks,
            technicalIssuesCount: feedbacks.Count(f => f.HadTechnicalIssues)
        );

        return Result.Success(summary);
    }
}
