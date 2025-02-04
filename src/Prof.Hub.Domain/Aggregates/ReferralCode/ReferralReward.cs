using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.ReferralCode;
public class ReferralReward : AuditableEntity
{
    public ReferralRewardId Id { get; private set; }
    public ReferralInviteId ReferralInviteId { get; private set; }
    public StudentId StudentId { get; private set; }
    public RewardAmount Amount { get; private set; }
    public RewardStatus Status { get; private set; }
    public DateTime? ProcessedAt { get; private set; }

    internal ReferralReward() { }

    internal static Result<ReferralReward> Create(
        ReferralInviteId referralInviteId,
        StudentId studentId,
        RewardAmount amount)
    {
        var reward = new ReferralReward
        {
            Id = ReferralRewardId.Create(),
            ReferralInviteId = referralInviteId,
            StudentId = studentId,
            Amount = amount,
            Status = RewardStatus.Pending,
            Created = DateTime.UtcNow
        };

        return reward;
    }

    internal Result Process()
    {
        if (Status != RewardStatus.Pending)
            return Result.Invalid(new ValidationError("A recompensa não está pendente"));

        Status = RewardStatus.Processed;
        ProcessedAt = DateTime.UtcNow;

        return Result.Success();
    }
}
