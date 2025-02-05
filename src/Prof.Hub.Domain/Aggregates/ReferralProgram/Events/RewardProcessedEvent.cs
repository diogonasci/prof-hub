using Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
public record RewardProcessedEvent(ReferralRewardId RewardId, StudentId StudentId, decimal Amount) : IDomainEvent;

