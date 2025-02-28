using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
using Prof.Hub.Domain.Aggregates.ReferralProgram.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram;
public class ReferralProgram : AuditableEntity, IAggregateRoot
{
    private readonly List<ReferralCode.ReferralCode> _referralCodes = [];
    private readonly List<RewardHistory> _rewardHistory = [];

    private const int MAX_CODES_PER_REFERRER = 5;
    private const int MAX_VALIDITY_DAYS = 90;

    public ReferralProgramId Id { get; private set; }
    public RewardAmount ReferrerReward { get; private set; }
    public RewardAmount ReferredReward { get; private set; }
    public ExpirationPeriod CodeValidityPeriod { get; private set; }
    public bool IsActive { get; private set; }
    public Money Currency { get; private set; }

    public IReadOnlyCollection<ReferralCode.ReferralCode> ReferralCodes => _referralCodes.AsReadOnly();
    public IReadOnlyCollection<RewardHistory> RewardHistoryList => _rewardHistory.AsReadOnly();

    private ReferralProgram() { }

    public static Result<ReferralProgram> Create(
        RewardAmount referrerReward,
        RewardAmount referredReward,
        ExpirationPeriod codeValidityPeriod,
        Money currency)
    {
        var errors = new List<ValidationError>();

        if (codeValidityPeriod.Value.TotalDays > MAX_VALIDITY_DAYS)
            errors.Add(new ValidationError($"Período de validade não pode exceder {MAX_VALIDITY_DAYS} dias."));

        var initialHistoryResult = RewardHistory.Create(referrerReward, referredReward, DateTime.UtcNow);

        if (!initialHistoryResult.IsSuccess)
            errors.AddRange(initialHistoryResult.ValidationErrors);

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var program = new ReferralProgram
        {
            Id = ReferralProgramId.Create(),
            ReferrerReward = referrerReward,
            ReferredReward = referredReward,
            CodeValidityPeriod = codeValidityPeriod,
            Currency = currency,
            IsActive = true
        };

        program._rewardHistory.Add(initialHistoryResult.Value);

        return program;
    }

    public Result<ReferralCode.ReferralCode> GenerateCode(StudentId referrerId)
    {
        var errors = new List<ValidationError>();

        if (!IsActive)
            errors.Add(new ValidationError("O programa de indicação não está ativo."));

        var activeCodes = _referralCodes.Count(c =>
            c.ReferrerId == referrerId &&
            c.ExpiresAt > DateTime.UtcNow &&
            c.IsActive);

        if (activeCodes >= MAX_CODES_PER_REFERRER)
            errors.Add(new ValidationError($"Limite de {MAX_CODES_PER_REFERRER} códigos ativos por usuário."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var codeResult = ReferralCode.ReferralCode.Generate(
            referrerId,
            DateTime.UtcNow.Add(CodeValidityPeriod.Value)
        );

        if (codeResult.IsSuccess)
        {
            _referralCodes.Add(codeResult.Value);
            AddDomainEvent(new ReferralCodeGeneratedEvent(Id.Value, codeResult.Value.Id.Value));
        }

        return codeResult;
    }

    public Result UpdateRewards(RewardAmount newReferrerReward, RewardAmount newReferredReward)
    {
        if (!IsActive)
            return Result.Invalid(new ValidationError("Não é possível atualizar recompensas de um programa inativo."));

        var historyResult = RewardHistory.Create(newReferrerReward, newReferredReward, DateTime.UtcNow);

        if (!historyResult.IsSuccess)
            return Result.Invalid(historyResult.ValidationErrors);

        ReferrerReward = newReferrerReward;
        ReferredReward = newReferredReward;
        _rewardHistory.Add(historyResult.Value);

        AddDomainEvent(new ReferralProgramRewardsUpdatedEvent(Id.Value, newReferrerReward.Value, newReferredReward.Value));

        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return Result.Invalid(new ValidationError("Programa já está inativo."));

        IsActive = false;

        // Desativa todos os códigos ativos
        foreach (var code in _referralCodes.Where(c => c.IsActive))
        {
            var result = code.Deactivate();
            if (!result.IsSuccess)
                return result;
        }

        AddDomainEvent(new ReferralProgramDeactivatedEvent(Id.Value));

        return Result.Success();
    }

    public bool HasReferredStudent(Email studentEmail)
    {
        return _referralCodes
            .SelectMany(c => c.Invites)
            .Any(i => i.ReferredEmail == studentEmail && i.Status == InviteStatus.Accepted);
    }

    public RewardHistory GetRewardsAt(DateTime date)
    {
        var history = _rewardHistory
            .Where(h => h.UpdatedAt <= date)
            .OrderByDescending(h => h.UpdatedAt)
            .FirstOrDefault();

        return history;
    }

    public IEnumerable<ReferralCode.ReferralCode> GetActiveCodes()
        => _referralCodes.Where(c => c.ExpiresAt > DateTime.UtcNow && c.IsActive);

    public IEnumerable<ReferralCode.ReferralCode> GetExpiredCodes()
        => _referralCodes.Where(c => c.ExpiresAt <= DateTime.UtcNow || !c.IsActive);
}
