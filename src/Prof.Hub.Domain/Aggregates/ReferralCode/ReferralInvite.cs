using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.ReferralCode;
public class ReferralInvite : AuditableEntity
{
    private readonly List<ReferralReward> _rewards = [];

    public ReferralInviteId Id { get; private set; }
    public ReferralCodeId ReferralCodeId { get; private set; }
    public StudentId ReferrerId { get; private set; }
    public Email ReferredEmail { get; private set; }
    public InviteStatus Status { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public IReadOnlyList<ReferralReward> Rewards => _rewards.AsReadOnly();

    private ReferralInvite()
    {
    }

    public static Result<ReferralInvite> Create(ReferralCodeId referralCodeId, StudentId referrerId, Email referredEmail)
    {
        var invite = new ReferralInvite
        {
            Id = ReferralInviteId.Create(),
            ReferralCodeId = referralCodeId,
            ReferrerId = referrerId,
            ReferredEmail = referredEmail,
            Status = InviteStatus.Pending,
            Created = DateTime.UtcNow
        };

        return invite;
    }

    internal Result Accept()
    {
        if (Status != InviteStatus.Pending)
            return Result.Invalid(new ValidationError("O convite não está pendente"));

        Status = InviteStatus.Accepted;
        CompletedAt = DateTime.UtcNow;

        return Result.Success();
    }

    internal Result<ReferralReward> AddReward(RewardAmount amount, StudentId studentId)
    {
        var rewardResult = ReferralReward.Create(Id, studentId, amount);

        var errors = new List<ValidationError>();

        if (Status != InviteStatus.Accepted)
            errors.Add(new ValidationError("O convite precisa ser aceito para adicionar recompensas"));

        if (!rewardResult.IsSuccess)
            errors.AddRange(rewardResult.ValidationErrors);

        if (errors.Count > 0)
            return Result.Invalid(errors);

        _rewards.Add(rewardResult.Value);

        return rewardResult;
    }
}

