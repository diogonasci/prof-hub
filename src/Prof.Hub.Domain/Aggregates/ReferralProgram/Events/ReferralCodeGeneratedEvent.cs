using Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
using Prof.Hub.Domain.Aggregates.ReferralProgram.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
public record ReferralCodeGeneratedEvent(ReferralProgramId ProgramId, ReferralCodeId CodeId) : IDomainEvent;

