using Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Events;
public record MaterialAddedEvent(string ClassId, ClassMaterialId MaterialId) : IDomainEvent;
