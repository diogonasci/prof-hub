using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.ReferralProgram.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.ReferralProgram;
public class ReferralProgram : AuditableEntity, IAggregateRoot
{
    private readonly List<ReferralCode.ReferralCode> _referralCodes = [];

    public ReferralProgramId Id { get; private set; }
    public RewardAmount ReferrerReward { get; private set; }
    public RewardAmount ReferredReward { get; private set; }
    public ExpirationPeriod CodeValidityPeriod { get; private set; }
    public bool IsActive { get; private set; }
    public IReadOnlyList<ReferralCode.ReferralCode> ReferralCodes => _referralCodes.AsReadOnly();

    private ReferralProgram()
    {
    }

    public static Result<ReferralProgram> Create(
        RewardAmount referrerReward,
        RewardAmount referredReward,
        ExpirationPeriod codeValidityPeriod)
    {
        var program = new ReferralProgram
        {
            Id = ReferralProgramId.Create(),
            ReferrerReward = referrerReward,
            ReferredReward = referredReward,
            CodeValidityPeriod = codeValidityPeriod,
            IsActive = true
        };

        return program;
    }

    public Result<ReferralCode.ReferralCode> GenerateCode(StudentId referrerId)
    {
        if (!IsActive)
            return Result.Invalid(new ValidationError("O programa de indicação não está ativo."));

        var codeResult = ReferralCode.ReferralCode.Generate(
            referrerId,
            DateTime.UtcNow.Add(CodeValidityPeriod.Value)
        );

        if (codeResult.IsSuccess)
        {
            _referralCodes.Add(codeResult.Value);
            AddDomainEvent(new ReferralCodeGeneratedEvent(Id, codeResult.Value.Id));
        }

        return codeResult;
    }
}
