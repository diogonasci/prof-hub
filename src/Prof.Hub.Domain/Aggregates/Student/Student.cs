using Prof.Hub.Domain.Aggregates.Common;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;


namespace Prof.Hub.Domain.Aggregates.Student
{
    public class Student : AuditableEntity
    {
        private readonly List<PrivateLesson.PrivateLesson> _scheduledLessons = [];

        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public Address Address { get; private set; }
        public Parent Parent { get; private set; }
        public ClassHours ClassHours { get; private set; }

        public IReadOnlyList<PrivateLesson.PrivateLesson> ScheduledLessons => _scheduledLessons.AsReadOnly();

        private Student()
        {
        }

        public static Student Create(Name name, Email email, PhoneNumber phoneNumber, Address address, Parent parent, ClassHours classHours)
        {
            var student = new Student
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address,
                Parent = parent,
                ClassHours = classHours
            };

            return student;
        }

        public void AddClassHours(int hours)
        {
            ClassHours = ClassHours.Add(hours);
        }

        public Result ScheduleLesson(PrivateLesson.PrivateLesson newLesson)
        {
            if (newLesson == null)
                return Result.Invalid(new ValidationError("A nova aula não pode ser nula."));

            _scheduledLessons.Add(newLesson);
            return Result.Success();
        }

        public Result CancelLesson(PrivateLesson.PrivateLesson lessonToCancel)
        {
            if (lessonToCancel == null)
                return Result.Invalid(new ValidationError("A aula a ser cancelada não pode ser nula."));

            if (!_scheduledLessons.Remove(lessonToCancel))
                return Result.Invalid(new ValidationError("A aula não está agendada."));

            return Result.Success();
        }
    }
}
