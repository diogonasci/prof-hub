using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Payment.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Payment;
public class Payment : AuditableEntity, IAggregateRoot
{
    public PaymentId Id { get; private set; }
    public StudentId StudentId { get; private set; }
    public Money Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public CardInfo? CardInfo { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private Payment(PaymentId id, StudentId studentId, Money amount, CardInfo? cardInfo)
    {
        Id = id;
        StudentId = studentId;
        Amount = amount;
        CardInfo = cardInfo;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<Payment> Create(StudentId userId, Money amount, CardInfo? cardInfo = null)
    {
        var errors = new List<ValidationError>();

        if (amount.Amount <= 0)
            errors.Add(new ValidationError("O valor do pagamento deve ser maior que zero"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        return new Payment(PaymentId.Create(), userId, amount, cardInfo);
    }

    public Result Process()
    {
        if (Status != PaymentStatus.Pending)
            return Result.Invalid(new ValidationError("O pagamento só pode ser processado quando estiver pendente"));

        Status = PaymentStatus.Processing;

        return Result.Success();
    }

    public Result Complete()
    {
        if (Status != PaymentStatus.Processing)
            return Result.Invalid(new ValidationError("O pagamento só pode ser concluído quando estiver em processamento"));

        Status = PaymentStatus.Completed;
        CompletedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result Fail()
    {
        if (Status != PaymentStatus.Processing)
            return Result.Invalid(new ValidationError("O pagamento só pode falhar quando estiver em processamento"));

        Status = PaymentStatus.Failed;

        return Result.Success();
    }
}
