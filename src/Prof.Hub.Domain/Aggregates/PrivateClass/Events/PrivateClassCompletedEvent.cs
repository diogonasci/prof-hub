using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record PrivateClassCompletedEvent(
    string ClassId,
    string StudentId,
    int OverallRating,
    int TeachingRating,
    int MaterialsRating,
    int TechnicalRating,
    string? TeacherComment,
    string? TechnicalComment,
    bool IsAnonymous,
    bool HadTechnicalIssues
) : IDomainEvent;
