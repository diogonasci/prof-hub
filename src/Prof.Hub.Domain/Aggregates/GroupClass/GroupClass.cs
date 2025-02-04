using Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback;
using Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial;
using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.GroupClass
{
    public class GroupClass : AuditableEntity, IAggregateRoot
    {
        private readonly List<ClassMaterial> _materials = [];
        private readonly List<ClassFeedback> _feedbacks = [];

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

        public Result EnrollStudent(StudentId studentId)
        {
            var errors = new List<ValidationError>();

            if (Status != ClassStatus.Published)
                errors.Add(new ValidationError("A aula não está aberta para inscrições."));

            if (Participants.Count >= ParticipantLimit.Value)
                errors.Add(new ValidationError("As vagas para está aula esgotaram."));

            if (errors.Count > 0)
                return Result.Invalid(errors);

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
