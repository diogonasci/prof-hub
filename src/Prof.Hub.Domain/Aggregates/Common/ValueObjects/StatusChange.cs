using Prof.Hub.Domain.Enums;

namespace Prof.Hub.Domain.Aggregates.Common.ValueObjects;
public record StatusChange(ClassStatus PreviousStatus, ClassStatus NewStatus, DateTime ChangedAt);
