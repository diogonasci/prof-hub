using Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record PrivateClassCompletedEvent(string ClassId, string StudentId, ClassFeedback Feedback) : IDomainEvent;
