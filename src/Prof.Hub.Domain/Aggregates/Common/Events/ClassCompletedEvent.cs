using Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Events;
public record ClassCompletedEvent(string ClassId, ClassFeedback Feedback) : IDomainEvent;
