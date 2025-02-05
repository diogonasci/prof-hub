using Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial.Events;
public record MaterialUploadedEvent(ClassMaterialId MaterialId, string Title) : IDomainEvent;

