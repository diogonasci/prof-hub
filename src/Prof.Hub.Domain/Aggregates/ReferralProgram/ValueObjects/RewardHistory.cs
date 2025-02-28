using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram.ValueObjects;
public sealed record RewardHistory
{
    public RewardAmount ReferrerReward { get; }
    public RewardAmount ReferredReward { get; }
    public DateTime UpdatedAt { get; }

    private RewardHistory(RewardAmount referrerReward, RewardAmount referredReward, DateTime updatedAt)
    {
        ReferrerReward = referrerReward;
        ReferredReward = referredReward;
        UpdatedAt = updatedAt;
    }

    public static Result<RewardHistory> Create(
        RewardAmount referrerReward,
        RewardAmount referredReward,
        DateTime updatedAt)
    {
        var errors = new List<ValidationError>();

        if (referrerReward is null)
            errors.Add(new ValidationError("Recompensa do referente é obrigatória"));

        if (referredReward is null)
            errors.Add(new ValidationError("Recompensa do referido é obrigatória"));

        if (updatedAt > DateTime.UtcNow)
            errors.Add(new ValidationError("Data de atualização não pode estar no futuro"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        return new RewardHistory(referrerReward, referredReward, updatedAt);
    }
}
