using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.GroupClass.ValueObjects;
public sealed record GroupDiscount
{
    public int MinParticipants { get; }
    public decimal PercentageDiscount { get; }

    private GroupDiscount(int minParticipants, decimal percentageDiscount)
    {
        MinParticipants = minParticipants;
        PercentageDiscount = percentageDiscount;
    }

    public static Result<GroupDiscount> Create(int minParticipants, decimal percentageDiscount)
    {
        var errors = new List<ValidationError>();

        if (minParticipants < 2)
            errors.Add(new ValidationError("Mínimo de participantes deve ser maior que 1."));

        if (percentageDiscount <= 0 || percentageDiscount > 100)
            errors.Add(new ValidationError("Percentual de desconto deve estar entre 0 e 100."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        return new GroupDiscount(minParticipants, percentageDiscount);
    }
}
