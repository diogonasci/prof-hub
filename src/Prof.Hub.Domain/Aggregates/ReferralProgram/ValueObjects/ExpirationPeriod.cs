using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.ValueObjects;
public record ExpirationPeriod
{
    public TimeSpan Value { get; }

    private ExpirationPeriod(TimeSpan value) => Value = value;

    public static Result<ExpirationPeriod> Create(TimeSpan value)
    {
        if (value <= TimeSpan.Zero)
            return Result.Invalid(new ValidationError("O valor deve ser positivo."));

        return new ExpirationPeriod(value);
    }
}
