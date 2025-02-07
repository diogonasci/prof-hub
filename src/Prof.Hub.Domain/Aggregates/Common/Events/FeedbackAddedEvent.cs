using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Events;
public record FeedbackAddedEvent(string ClassId, string FeedbackId) : IDomainEvent;
