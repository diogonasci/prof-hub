using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Events;
public record ClassCompletedEvent(
    string ClassId,
    int OverallRating,
    int TeachingRating,
    int MaterialsRating,
    int TechnicalRating,
    string TeacherComment,
    string TechnicalComment,
    bool IsAnonymous,
    bool HadTechnicalIssues,
    DateTime CompletedAt) : IDomainEvent;
