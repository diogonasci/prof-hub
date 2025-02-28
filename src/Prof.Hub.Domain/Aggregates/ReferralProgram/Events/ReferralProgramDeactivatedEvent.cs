using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
public record ReferralProgramDeactivatedEvent(string ProgramId) : IDomainEvent;
