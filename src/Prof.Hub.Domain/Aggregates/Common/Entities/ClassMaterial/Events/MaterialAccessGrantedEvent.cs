using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial.Events;
public record MaterialAccessGrantedEvent(string MaterialId, string StudentId) : IDomainEvent;

