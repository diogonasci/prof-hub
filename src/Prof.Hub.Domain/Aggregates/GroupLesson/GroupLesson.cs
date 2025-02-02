﻿using Prof.Hub.Domain.Aggregates.PrivateLesson.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.GroupLesson
{
    public class GroupLesson : AuditableEntity
    {
        private readonly List<Student.Student> _students = [];

        public Guid TeacherId { get; private set; }
        public Price Price { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public ClassStatus Status { get; private set; }

        public IReadOnlyList<Student.Student> Students => _students.AsReadOnly();

        private GroupLesson(Guid teacherId, Price price, DateTime startTime, DateTime endTime)
        {
            Id = Guid.NewGuid();
            TeacherId = teacherId;
            Price = price;
            StartTime = startTime;
            EndTime = endTime;
            Status = ClassStatus.Scheduled;
        }

        public static Result<GroupLesson> Create(Guid teacherId, Price price, DateTime startTime, DateTime endTime)
        {
            if (startTime >= endTime)
                return Result.Invalid(new ValidationError("O horário de início deve ser anterior ao horário de término."));

            var lesson = new GroupLesson(teacherId, price, startTime, endTime);
            return Result.Success(lesson);
        }

        public Result Cancel()
        {
            if (Status != ClassStatus.Scheduled)
                return Result.Invalid(new ValidationError("Apenas aulas agendadas podem ser canceladas."));

            Status = ClassStatus.Canceled;
            return Result.Success();
        }
    }
}
