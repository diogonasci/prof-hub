using Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback;
using Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record PrivateClassCompletedEvent(PrivateClassId ClassId, StudentId StudentId, ClassFeedback Feedback) : IDomainEvent;
