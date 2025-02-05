using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.Events;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student.Entities;
public class EnrollmentHistory : Entity
{
    public Guid Id { get; private set; }
    public StudentId StudentId { get; private set; }
    public string ClassId { get; private set; }  // Pode ser GroupClassId ou PrivateClassId
    public string ClassType { get; private set; }  // "Group" ou "Private"
    public ClassStatus Status { get; private set; }
    public DateTime EnrolledAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public Rating? Rating { get; private set; }

    private EnrollmentHistory() { }

    public static EnrollmentHistory CreateForGroupClass(
        StudentId studentId,
        GroupClassId classId,
        ClassStatus status)
    {
        return new EnrollmentHistory
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            ClassId = classId.Value,
            ClassType = "Group",
            Status = status,
            EnrolledAt = DateTime.UtcNow
        };
    }

    public static EnrollmentHistory CreateForPrivateClass(
        StudentId studentId,
        PrivateClassId classId,
        ClassStatus status)
    {
        return new EnrollmentHistory
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            ClassId = classId.Value,
            ClassType = "Private",
            Status = status,
            EnrolledAt = DateTime.UtcNow
        };
    }

    public void Complete(Rating rating)
    {
        Status = ClassStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        Rating = rating;
        AddDomainEvent(new EnrollmentCompletedEvent(StudentId, ClassId, Rating));
    }

    public void Cancel()
    {
        Status = ClassStatus.Cancelled;
        AddDomainEvent(new EnrollmentCanceledEvent(StudentId, ClassId));
    }
}
