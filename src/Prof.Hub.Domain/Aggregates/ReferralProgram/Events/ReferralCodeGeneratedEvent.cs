using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
public record ReferralCodeGeneratedEvent(string ProgramId, string CodeId) : IDomainEvent;

