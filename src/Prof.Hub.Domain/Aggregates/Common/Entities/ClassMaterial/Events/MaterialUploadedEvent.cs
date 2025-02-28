using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial.Events;
public record MaterialUploadedEvent(string MaterialId, string Title) : IDomainEvent;

