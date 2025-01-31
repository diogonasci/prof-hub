using Prof.Hub.Domain.Aggregates.Common;
using Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Teacher
{
    public class Teacher : AuditableEntity
    {
        private readonly List<PrivateLesson.PrivateLesson> _privateLessons = [];
        private readonly List<GroupLesson.GroupLesson> _groupLessons = [];

        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public Address Address { get; private set; }
        public HourlyRate HourlyRate { get; private set; }

        public IReadOnlyList<PrivateLesson.PrivateLesson> PrivateLessons => _privateLessons.AsReadOnly();
        public IReadOnlyList<GroupLesson.GroupLesson> GroupLessons => _groupLessons.AsReadOnly();

        private Teacher() { }

        public static Result<Teacher> Create(Name name, Email email, PhoneNumber phoneNumber, Address address, HourlyRate hourlyRate)
        {
            var teacher = new Teacher
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address,
                HourlyRate = hourlyRate
            };

            return teacher;
        }

        public Result AddScheduledPrivateLesson(PrivateLesson.PrivateLesson lesson)
        {
            if (lesson == null)
                return Result.Invalid(new ValidationError("A aula particular não pode ser nula."));

            if (lesson.TeacherId != Id)
                return Result.Invalid(new ValidationError("Esta aula particular não pertence a este professor."));

            _privateLessons.Add(lesson);
            return Result.Success();
        }

        public Result RemoveScheduledPrivateLesson(PrivateLesson.PrivateLesson lesson)
        {
            if (lesson == null)
                return Result.Invalid(new ValidationError("A aula particular a ser cancelada não pode ser nula."));

            if (!_privateLessons.Remove(lesson))
                return Result.Invalid(new ValidationError("A aula particular não está agendada para este professor."));

            return lesson.Cancel();
        }

        public Result AddScheduledGroupLesson(GroupLesson.GroupLesson lesson)
        {
            if (lesson == null)
                return Result.Invalid(new ValidationError("A aula em grupo não pode ser nula."));

            if (lesson.TeacherId != Id)
                return Result.Invalid(new ValidationError("Esta aula em grupo não pertence a este professor."));

            _groupLessons.Add(lesson);
            return Result.Success();
        }

        public Result RemoveScheduledGroupLesson(GroupLesson.GroupLesson lesson)
        {
            if (lesson == null)
                return Result.Invalid(new ValidationError("A aula em grupo a ser cancelada não pode ser nula."));

            if (!_groupLessons.Remove(lesson))
                return Result.Invalid(new ValidationError("A aula em grupo não está agendada para este professor."));

            return lesson.Cancel();
        }
    }
}
