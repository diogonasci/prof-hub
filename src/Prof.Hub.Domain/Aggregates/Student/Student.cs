﻿using Prof.Hub.Domain.Aggregates.Common;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;


namespace Prof.Hub.Domain.Aggregates.Student
{
    public class Student : AuditableEntity
    {
        private readonly List<PrivateLesson.PrivateLesson> _privateLessons = [];
        private readonly List<GroupLesson.GroupLesson> _groupLessons = [];

        public Name Name { get; private set; }
        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public Address Address { get; private set; }
        public Parent Parent { get; private set; }
        public ClassHours ClassHours { get; private set; }

        public IReadOnlyList<PrivateLesson.PrivateLesson> PrivateLessons => _privateLessons.AsReadOnly();
        public IReadOnlyList<GroupLesson.GroupLesson> GroupLessons => _groupLessons.AsReadOnly();


        private Student()
        {
        }

        public static Result<Student> Create(
            string name,
            string email,
            string phoneNumber,
            string street,
            string city,
            string state,
            string postalCode,
            int classHoursValue
        )
        {
            var nameResult = Name.Create(name);
            var emailResult = Email.Create(email);
            var phoneResult = PhoneNumber.Create(phoneNumber);
            var addressResult = Address.Create(street, city, state, postalCode);
            var classHoursResult = ClassHours.Create(classHoursValue);

            if (!nameResult.IsSuccess || !emailResult.IsSuccess || !phoneResult.IsSuccess || !addressResult.IsSuccess ||
                !classHoursResult.IsSuccess)
            {
                var errors = new List<ValidationError>();

                if (nameResult.ValidationErrors.Any()) errors.AddRange(nameResult.ValidationErrors);
                if (emailResult.ValidationErrors.Any()) errors.AddRange(emailResult.ValidationErrors);
                if (phoneResult.ValidationErrors.Any()) errors.AddRange(phoneResult.ValidationErrors);
                if (addressResult.ValidationErrors.Any()) errors.AddRange(addressResult.ValidationErrors);
                if (classHoursResult.ValidationErrors.Any()) errors.AddRange(classHoursResult.ValidationErrors);

                return Result.Invalid(errors);
            }

            var student = new Student
            {
                Id = Guid.NewGuid(),
                Name = nameResult.Value,
                Email = emailResult.Value,
                PhoneNumber = phoneResult.Value,
                Address = addressResult.Value,
                Parent = null,
                ClassHours = classHoursResult.Value
            };

            return Result.Success(student);
        }


        public void AddClassHours(int hours)
        {
            ClassHours = ClassHours.Add(hours);
        }

        public Result SchedulePrivateLesson(PrivateLesson.PrivateLesson newLesson)
        {
            if (newLesson == null)
                return Result.Invalid(new ValidationError("A nova aula particular não pode ser nula."));

            var hasConflict = _privateLessons.Any(lesson =>
                lesson.StartTime < newLesson.EndTime && newLesson.StartTime < lesson.EndTime);

            if (hasConflict)
                return Result.Invalid(new ValidationError("Conflito de horário detectado para aula particular."));

            _privateLessons.Add(newLesson);
            return Result.Success();
        }

        public Result CancelPrivateLesson(PrivateLesson.PrivateLesson lessonToCancel)
        {
            if (lessonToCancel == null)
                return Result.Invalid(new ValidationError("A aula particular a ser cancelada não pode ser nula."));

            if (!_privateLessons.Remove(lessonToCancel))
                return Result.Invalid(new ValidationError("A aula particular não está agendada."));

            return Result.Success();
        }

        public Result JoinGroupLesson(GroupLesson.GroupLesson lesson)
        {
            if (lesson == null)
                return Result.Invalid(new ValidationError("A aula em grupo não pode ser nula."));

            if (_groupLessons.Contains(lesson))
                return Result.Invalid(new ValidationError("O aluno já está matriculado nesta aula em grupo."));

            _groupLessons.Add(lesson);
            return Result.Success();
        }

        public Result LeaveGroupLesson(GroupLesson.GroupLesson lesson)
        {
            if (lesson == null)
                return Result.Invalid(new ValidationError("A aula em grupo não pode ser nula."));

            if (!_groupLessons.Remove(lesson))
                return Result.Invalid(new ValidationError("O aluno não está nessa aula em grupo."));

            return Result.Success();
        }
    }
}
