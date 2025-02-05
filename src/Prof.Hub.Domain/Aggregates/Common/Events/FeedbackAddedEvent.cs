using Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Events;
public record FeedbackAddedEvent(string ClassId, ClassFeedbackId FeedbackId) : IDomainEvent;
