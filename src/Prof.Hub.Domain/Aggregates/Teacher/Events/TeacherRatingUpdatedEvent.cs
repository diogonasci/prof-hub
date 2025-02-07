using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Teacher.Events;
public record TeacherRatingUpdatedEvent(string TeacherId, int NewRating) : IDomainEvent;

