using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
public sealed record ParticipantLimit
{
    public int Value { get; }

    private ParticipantLimit(int value)
    {
        Value = value;
    }

    public static Result<ParticipantLimit> Create(int value)
    {
        if (value < 1 || value > 100)
            return Result.Invalid(new ValidationError("O limite de participantes deve estar entre 1 e 100."));

        return Result.Success(new ParticipantLimit(value));
    }
}
