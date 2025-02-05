using Prof.Hub.Domain.Aggregates.Student.Events;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.Student.Entities;
public class TeacherFavorite : Entity
{
    public StudentId StudentId { get; private set; }
    public TeacherId TeacherId { get; private set; }
    public DateTime AddedAt { get; private set; }
    public string? Note { get; private set; }

    public TeacherFavorite(StudentId studentId, TeacherId teacherId, string? note = null)
    {
        StudentId = studentId;
        TeacherId = teacherId;
        AddedAt = DateTime.UtcNow;
        Note = note;

        AddDomainEvent(new TeacherAddedToFavoritesEvent(studentId, teacherId));
    }

    public void UpdateNote(string note)
    {
        Note = note;
        AddDomainEvent(new TeacherFavoriteNoteUpdatedEvent(StudentId, TeacherId, note));
    }
}
