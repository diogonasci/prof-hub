using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.ReferralCode.Entities;
using Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
using Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
using Prof.Hub.Domain.Aggregates.ReferralProgram.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.ReferralCode;
public class ReferralCode : AuditableEntity, IAggregateRoot
{
    private readonly List<ReferralInvite> _invites = [];

    public ReferralCodeId Id { get; private set; }
    public ReferralCodeValue Code { get; private set; }
    public StudentId ReferrerId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsActive { get; private set; }
    public IReadOnlyList<ReferralInvite> Invites => _invites.AsReadOnly();

    private ReferralCode()
    {
    }

    public static Result<ReferralCode> Generate(StudentId referrerId, DateTime expiresAt)
    {
        var codeValue = GenerateUniqueCode();
        var codeResult = ReferralCodeValue.Create(codeValue);

        if (!codeResult.IsSuccess)
            return Result.Invalid(codeResult.ValidationErrors);

        var referralCode = new ReferralCode
        {
            Id = ReferralCodeId.Create(),
            Code = codeResult.Value,
            ReferrerId = referrerId,
            ExpiresAt = expiresAt,
            IsActive = true
        };

        return referralCode;
    }

    public Result<ReferralInvite> CreateInvite(Email referredEmail)
    {
        var errors = new List<ValidationError>();

        var inviteResult = ReferralInvite.Create(Id, ReferrerId, referredEmail);

        if (!IsActive || DateTime.UtcNow > ExpiresAt)
            errors.Add(new ValidationError("O código de indicação não é válido."));

        if (!inviteResult.IsSuccess)
            errors.AddRange(inviteResult.ValidationErrors);

        if (errors.Count > 0)
            return Result.Invalid(errors);

        _invites.Add(inviteResult.Value);
        AddDomainEvent(new InviteSentEvent(Id, inviteResult.Value.Id));

        return inviteResult;
    }

    private static string GenerateUniqueCode()
    {
        return Guid.NewGuid().ToString()[..8].ToUpper();
    }
}
