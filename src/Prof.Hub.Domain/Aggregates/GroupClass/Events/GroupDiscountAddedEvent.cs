using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.GroupClass.Events;
public record GroupDiscountAddedEvent(
    string ClassId,
    int MinParticipants,
    decimal PercentageDiscount) : IDomainEvent;
