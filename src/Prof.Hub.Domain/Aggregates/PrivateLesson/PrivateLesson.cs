using Prof.Hub.Domain.Aggregates.PrivateLesson.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.PrivateLesson
{
    public class PrivateLesson : AuditableEntity
    {
        public Guid TeacherId { get; private set; }
        public Guid StudentId { get; private set; }
        public Price Price { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public ClassStatus Status { get; private set; }

        private PrivateLesson(Guid teacherId, Guid studentId, Price price, DateTime startTime, DateTime endTime)
        {
            Id = Guid.NewGuid();
            TeacherId = teacherId;
            StudentId = studentId;
            Price = price;
            StartTime = startTime;
            EndTime = endTime;
            Status = ClassStatus.Scheduled;
        }

        public static Result<PrivateLesson> Create(Guid teacherId, Guid studentId, Price price, DateTime startTime, DateTime endTime)
        {
            var newLesson = new PrivateLesson(teacherId, studentId, price, startTime, endTime);
            return Result.Success(newLesson);
        }

        public Result Cancel()
        {
            if (Status != ClassStatus.Scheduled)
                return Result.Invalid(new ValidationError("Somente aulas agendadas podem ser canceladas."));

            Status = ClassStatus.Canceled;
            return Result.Success();
        }
    }
}
