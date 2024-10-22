using Prof.Hub.Domain.Aggregates.Common;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Teacher
{
    public class Teacher : AuditableEntity
    {
        private readonly List<PrivateLesson.PrivateLesson> _scheduledLessons = [];

        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public Address Address { get; private set; }
        public HourlyRate HourlyRate { get; private set; }

        public IReadOnlyList<PrivateLesson.PrivateLesson> ScheduledLessons => _scheduledLessons.AsReadOnly();

        private Teacher() { }

        public static Teacher Create(Name name, Email email, PhoneNumber phoneNumber, Address address, HourlyRate hourlyRate)
        {
            var teacher = new Teacher
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address,
                HourlyRate = hourlyRate
            };

            return teacher;
        }

        public Result ScheduleLesson(PrivateLesson.PrivateLesson lesson)
        {
            if (lesson == null)
                return Result.Invalid(new ValidationError("A aula não pode ser nula."));

            if (lesson.TeacherId != Id)
                return Result.Invalid(new ValidationError("Esta aula não pertence a este professor."));

            _scheduledLessons.Add(lesson);
            return Result.Success();
        }

        public Result CancelLesson(PrivateLesson.PrivateLesson lesson)
        {
            if (lesson == null)
                return Result.Invalid(new ValidationError("A aula a ser cancelada não pode ser nula."));

            if (!_scheduledLessons.Remove(lesson))
                return Result.Invalid(new ValidationError("A aula não está agendada para este professor."));

            var result = lesson.CancelLesson();
            if (!result.IsSuccess)
                return result;

            return Result.Success();
        }
    }
}
