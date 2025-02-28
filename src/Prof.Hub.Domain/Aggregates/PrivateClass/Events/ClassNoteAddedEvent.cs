using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record ClassNoteAddedEvent(
    string ClassId,
    string NoteContent) : IDomainEvent;
