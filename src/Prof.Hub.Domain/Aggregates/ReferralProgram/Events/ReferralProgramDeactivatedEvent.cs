using Prof.Hub.Domain.Aggregates.ReferralProgram.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
public record ReferralProgramDeactivatedEvent(ReferralProgramId ProgramId) : IDomainEvent;
