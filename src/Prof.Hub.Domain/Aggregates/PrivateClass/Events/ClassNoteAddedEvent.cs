using Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Domain.Aggregates.PrivateClass.Events;
public record ClassNoteAddedEvent(
    PrivateClassId ClassId,
    string NoteContent) : IDomainEvent;
