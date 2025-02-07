using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student.Events;
public record StudentBalanceUpdatedEvent(string StudentId, decimal NewBalance) : IDomainEvent;

