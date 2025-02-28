using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.ReferralCode.Entities;
using Prof.Hub.Domain.Aggregates.ReferralCode.Events;
using Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
using Prof.Hub.Domain.Aggregates.ReferralProgram.Events;
using Prof.Hub.Domain.Aggregates.ReferralProgram.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.ReferralCode;
public class ReferralCode : AuditableEntity, IAggregateRoot
{
    private readonly List<ReferralInvite> _invites = [];
    private const int MAX_INVITES = 10;
    private const int MIN_CODE_LENGTH = 8;

    public ReferralCodeId Id { get; private set; }
    public ReferralCodeValue Code { get; private set; }
    public StudentId ReferrerId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastUsedAt { get; private set; }
    public IReadOnlyList<ReferralInvite> Invites => _invites.AsReadOnly();

    private ReferralCode()
    {
    }

    public static Result<ReferralCode> Generate(StudentId referrerId, DateTime expiresAt)
    {
        var errors = new List<ValidationError>();

        if (expiresAt <= DateTime.UtcNow)
            errors.Add(new ValidationError("Data de expiração deve ser no futuro."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

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

        if (!IsActive)
            errors.Add(new ValidationError("O código de indicação não está ativo."));

        if (IsExpired())
            errors.Add(new ValidationError("O código de indicação expirou."));

        if (_invites.Count >= MAX_INVITES)
            errors.Add(new ValidationError($"Limite de {MAX_INVITES} convites por código atingido."));

        if (HasPendingInviteForEmail(referredEmail))
            errors.Add(new ValidationError("Já existe um convite pendente para este email."));

        if (IsReferrerEmail(referredEmail))
            errors.Add(new ValidationError("Não é possível enviar convite para o próprio referidor."));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var inviteResult = ReferralInvite.Create(Id, ReferrerId, referredEmail);

        if (!inviteResult.IsSuccess)
            return Result.Invalid(inviteResult.ValidationErrors);

        _invites.Add(inviteResult.Value);
        LastUsedAt = DateTime.UtcNow;

        AddDomainEvent(new InviteSentEvent(Id.Value, inviteResult.Value.Id.Value));

        return inviteResult;
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return Result.Invalid(new ValidationError("Código já está inativo."));

        IsActive = false;
        AddDomainEvent(new ReferralCodeDeactivatedEvent(Id.Value));

        return Result.Success();
    }

    public Result Reactivate()
    {
        if (IsActive)
            return Result.Invalid(new ValidationError("Código já está ativo."));

        if (IsExpired())
            return Result.Invalid(new ValidationError("Não é possível reativar um código expirado."));

        IsActive = true;
        AddDomainEvent(new ReferralCodeReactivatedEvent(Id.Value));

        return Result.Success();
    }

    public Result RenewExpiration(DateTime newExpirationDate)
    {
        if (newExpirationDate <= DateTime.UtcNow)
            return Result.Invalid(new ValidationError("Nova data de expiração deve ser no futuro."));

        if (!IsActive)
            return Result.Invalid(new ValidationError("Não é possível renovar um código inativo."));

        var oldExpirationDate = ExpiresAt;
        ExpiresAt = newExpirationDate;

        AddDomainEvent(new ReferralCodeExpirationRenewedEvent(Id.Value, oldExpirationDate, newExpirationDate));

        return Result.Success();
    }

    public CodeStatus GetStatus()
    {
        if (!IsActive) return CodeStatus.Inactive;
        if (IsExpired()) return CodeStatus.Expired;
        if (HasReachedInviteLimit()) return CodeStatus.FullyUsed;
        return CodeStatus.Active;
    }

    public IEnumerable<ReferralInvite> GetPendingInvites()
        => _invites.Where(i => i.Status == InviteStatus.Pending);

    public IEnumerable<ReferralInvite> GetAcceptedInvites()
        => _invites.Where(i => i.Status == InviteStatus.Accepted);

    private bool IsExpired() => DateTime.UtcNow > ExpiresAt;

    private bool HasReachedInviteLimit() => _invites.Count >= MAX_INVITES;

    private bool HasPendingInviteForEmail(Email email)
        => _invites.Any(i => i.ReferredEmail == email && i.Status == InviteStatus.Pending);

    private bool IsReferrerEmail(Email email)
        => ReferrerId.Value == email.Value;

    private static string GenerateUniqueCode()
    {
        return Guid.NewGuid().ToString()[..MIN_CODE_LENGTH].ToUpper();
    }
}
