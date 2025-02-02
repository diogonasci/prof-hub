using Prof.Hub.Domain.Aggregates.PrivateLesson.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;
using static Prof.Hub.Domain.Aggregates.Student.Student;

namespace Prof.Hub.Domain.Aggregates.GroupClass
{
    public class GroupClass : AuditableEntity
    {
        private readonly List<ClassMaterial> _materials = [];
        private readonly List<ClassReview> _reviews = [];

        public GroupClassId Id { get; private set; }
        public string Title { get; private set; }
        public string Slug { get; private set; }
        public TeacherId TeacherId { get; private set; }
        public Subject Subject { get; private set; }
        public ClassSchedule Schedule { get; private set; }
        public Price Price { get; private set; }
        public Uri ThumbnailUrl { get; private set; }
        public string Description { get; private set; }
        public ParticipantLimit ParticipantLimit { get; private set; }
        public HashSet<StudentId> Participants { get; private set; }
        public ClassStatus Status { get; private set; }

        public IReadOnlyList<ClassMaterial> Materials => _materials.AsReadOnly();
        public IReadOnlyList<ClassFeedback> Feedbacks => _feedbacks.AsReadOnly();

        private GroupClass()
        {
        }

        public record GroupClassId(string Value)
        {
            public static GroupClassId Create() => new(Guid.NewGuid().ToString());
        }

        public Result EnrollStudent(StudentId studentId)
        {
            if (Status != ClassStatus.Published)
                return Result.Invalid(new ValidationError("A aula não está aberta para inscrições."));

            if (Participants.Count >= ParticipantLimit.Value)
                return Result.Invalid(new ValidationError("As vagas para está aula esgotaram."));

            Participants.Add(studentId);
            AddDomainEvent(new StudentEnrolledEvent(Id, studentId));
            
            return Result.Success();
        }

        public Result Publish()
        {
            if (Status != ClassStatus.Draft)
                return Result.Invalid(new ValidationError("Somente aulas em rascunho podem ser publicadas."));

            Status = ClassStatus.Published;
            AddDomainEvent(new ThematicClassPublishedEvent(Id));

            return Result.Success();
        }
    }
}
