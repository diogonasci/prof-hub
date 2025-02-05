using Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial.Events;
public record MaterialAccessGrantedEvent(ClassMaterialId MaterialId, StudentId StudentId) : IDomainEvent;

