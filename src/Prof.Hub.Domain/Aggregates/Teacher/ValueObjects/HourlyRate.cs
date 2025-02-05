using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Teacher.ValueObjects;
public sealed record HourlyRate
{
    private const decimal MIN_RATE = 20.00m;
    private const decimal MAX_RATE = 500.00m;

    public Money Value { get; }
    public DateTime EffectiveFrom { get; }

    private HourlyRate(Money value, DateTime effectiveFrom)
    {
        Value = value;
        EffectiveFrom = effectiveFrom;
    }

    public static Result<HourlyRate> Create(Money value, DateTime effectiveFrom)
    {
        if (value.Amount < MIN_RATE)
            return Result.Invalid(new ValidationError($"Valor hora deve ser maior que {MIN_RATE}"));

        if (value.Amount > MAX_RATE)
            return Result.Invalid(new ValidationError($"Valor hora deve ser menor que {MAX_RATE}"));

        return new HourlyRate(value, effectiveFrom);
    }
}
