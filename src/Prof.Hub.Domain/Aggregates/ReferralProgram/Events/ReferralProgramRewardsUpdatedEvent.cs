using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
public record ReferralProgramRewardsUpdatedEvent(string ProgramId, decimal NewReferrerReward, decimal NewReferredReward) : IDomainEvent;
