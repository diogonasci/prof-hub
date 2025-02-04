using Prof.Hub.Domain.Aggregates.Common.Entities.ClassFeedback;
using Prof.Hub.Domain.Aggregates.Common.Entities.ClassMaterial;
using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.PrivateClass.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.PrivateClass
{
    public class PrivateClass : AuditableEntity, IAggregateRoot
    {
        private readonly List<ClassMaterial> _materials = [];
        private readonly List<ClassFeedback> _feedbacks = [];

        public PrivateClassId Id { get; private set; }
        public TeacherId TeacherId { get; private set; }
        public StudentId StudentId { get; private set; }
        public Subject Subject { get; private set; }
        public ClassSchedule Schedule { get; private set; }
        public Price Price { get; private set; }
        public ClassStatus Status { get; private set; }
        public Uri MeetingUrl { get; private set; }

        public IReadOnlyList<ClassMaterial> Materials => _materials.AsReadOnly();
        public IReadOnlyList<ClassFeedback> Feedbacks => _feedbacks.AsReadOnly();

        private PrivateClass()
        {
        }

        public static Result<PrivateClass> Create(
            TeacherId teacherId,
            StudentId studentId,
            Subject subject,
            DateTime startDate,
            TimeSpan duration,
            Price price,
            Uri meetingUrl)
        {
            var subjectResult = Subject.Create();
            var classScheduleResult = ClassSchedule.Create(startDate, duration);

            if (!subjectResult.IsSuccess || !classScheduleResult.IsSuccess)
            {
                var errors = new List<ValidationError>();

                if (subjectResult.ValidationErrors.Any()) 
                    errors.AddRange(subjectResult.ValidationErrors);

                if (classScheduleResult.ValidationErrors.Any()) 
                    errors.AddRange(classScheduleResult.ValidationErrors);

                return Result.Invalid(errors);
            }

            var privateClass = new PrivateClass
            {
                Id = PrivateClassId.Create(),
                TeacherId = TeacherId.Create(),
                StudentId = StudentId.Create(),
                Subject = subjectResult.Value,
                Schedule = classScheduleResult.Value
            };

            return privateClass;
        }

        public Result Start()
        {
            if (Status != ClassStatus.Scheduled)
                return Result.Invalid(new ValidationError("Somente aulas agendadas podem ser iniciadas."));

            Status = ClassStatus.InProgress;
            AddDomainEvent(new ClassStartedEvent(Id));

            return Result.Success();
        }

        public Result Complete(ClassFeedback feedback)
        {
            if (Status != ClassStatus.InProgress)
                return Result.Invalid(new ValidationError("Somente aulas em andamento podem ser completadas."));

            Status = ClassStatus.Completed;
            _feedbacks.Add(feedback);
            AddDomainEvent(new ClassCompletedEvent(Id, feedback));

            return Result.Success();
        }

        public Result Cancel()
        {
            if (Status != ClassStatus.Scheduled)
                return Result.Invalid(new ValidationError("Somente aulas agendadas podem ser canceladas."));

            Status = ClassStatus.Cancelled;
            AddDomainEvent(new ClassCanceledEvent(Id));

            return Result.Success();
        }
    }
}
