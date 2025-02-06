using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.ReferralProgram.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
public record ReferralProgramRewardsUpdatedEvent(ReferralProgramId ProgramId, RewardAmount NewReferrerReward, RewardAmount NewReferredReward) : IDomainEvent;
