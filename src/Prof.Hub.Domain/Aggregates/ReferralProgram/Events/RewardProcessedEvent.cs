using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
public record RewardProcessedEvent(string RewardId, string StudentId, decimal Amount) : IDomainEvent;

