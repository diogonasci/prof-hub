using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.UserAccount.Events;
using Prof.Hub.Domain.Aggregates.UserAccount.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.UserAccount;
public class UserAccount : AuditableEntity, IAggregateRoot
{
    private readonly List<LoginProvider> _loginProviders = [];
    private readonly List<NotificationPreference> _notificationPreferences = [];

    public UserAccountId Id { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber? PhoneNumber { get; private set; }
    public string PasswordHash { get; private set; }
    public Uri? AvatarUrl { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public bool IsPhoneVerified { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? DeactivatedAt { get; private set; }
    public string? DeactivationReason { get; private set; }

    public IReadOnlyList<LoginProvider> LoginProviders => _loginProviders.AsReadOnly();
    public IReadOnlyList<NotificationPreference> NotificationPreferences => _notificationPreferences.AsReadOnly();

    private UserAccount()
    {
        IsActive = true;
    }

    public static Result<UserAccount> Create(
        Email email,
        string passwordHash,
        Uri? avatarUrl = null)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(passwordHash))
            errors.Add(new ValidationError("Senha é obrigatória"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var account = new UserAccount
        {
            Id = UserAccountId.Create(),
            Email = email,
            PasswordHash = passwordHash,
            AvatarUrl = avatarUrl,
            Created = DateTime.UtcNow
        };

        account.AddDomainEvent(new UserAccountCreatedEvent(account.Id.Value));

        return account;
    }

    public Result UpdateEmail(Email newEmail)
    {
        if (newEmail == Email)
            return Result.Invalid(new ValidationError("Novo email é igual ao atual"));

        var previousEmail = Email;
        Email = newEmail;
        IsEmailVerified = false;

        AddDomainEvent(new UserEmailUpdatedEvent(Id.Value, previousEmail.Value, newEmail.Value));

        return Result.Success();
    }

    public Result UpdatePhone(PhoneNumber? newPhone)
    {
        var previousPhone = PhoneNumber;
        PhoneNumber = newPhone;
        IsPhoneVerified = false;

        if (newPhone != null)
            AddDomainEvent(new UserPhoneUpdatedEvent(Id.Value, previousPhone?.Value, newPhone.Value));

        return Result.Success();
    }

    public Result UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            return Result.Invalid(new ValidationError("Nova senha é obrigatória"));

        PasswordHash = newPasswordHash;

        AddDomainEvent(new UserPasswordUpdatedEvent(Id.Value));

        return Result.Success();
    }

    public Result UpdateAvatar(Uri? newAvatarUrl)
    {
        var previousUrl = AvatarUrl;
        AvatarUrl = newAvatarUrl;

        return Result.Success();
    }

    public Result AddLoginProvider(LoginProvider provider)
    {
        if (_loginProviders.Any(p => p.Type == provider.Type))
            return Result.Invalid(new ValidationError("Provedor já está vinculado à conta"));

        _loginProviders.Add(provider);
        AddDomainEvent(new LoginProviderAddedEvent(Id.Value, provider.Type));

        return Result.Success();
    }

    public Result RemoveLoginProvider(LoginProviderType type)
    {
        var provider = _loginProviders.FirstOrDefault(p => p.Type == type);
        if (provider == null)
            return Result.Invalid(new ValidationError("Provedor não está vinculado à conta"));

        if (_loginProviders.Count == 1 && string.IsNullOrEmpty(PasswordHash))
            return Result.Invalid(new ValidationError("Não é possível remover o único método de login"));

        _loginProviders.Remove(provider);
        AddDomainEvent(new LoginProviderRemovedEvent(Id.Value, type));

        return Result.Success();
    }

    public Result UpdateNotificationPreferences(NotificationType type, bool enabled)
    {
        var preference = _notificationPreferences.FirstOrDefault(p => p.Type == type);

        if (preference == null)
        {
            preference = new NotificationPreference(type, enabled);
            _notificationPreferences.Add(preference);
        }
        else
        {
            preference = preference with { IsEnabled = enabled };
        }

        return Result.Success();
    }

    public Result VerifyEmail()
    {
        if (IsEmailVerified)
            return Result.Invalid(new ValidationError("Email já está verificado"));

        IsEmailVerified = true;
        AddDomainEvent(new UserEmailVerifiedEvent(Id.Value));

        return Result.Success();
    }

    public Result VerifyPhone()
    {
        if (PhoneNumber == null)
            return Result.Invalid(new ValidationError("Nenhum telefone cadastrado"));

        if (IsPhoneVerified)
            return Result.Invalid(new ValidationError("Telefone já está verificado"));

        IsPhoneVerified = true;
        AddDomainEvent(new UserPhoneVerifiedEvent(Id.Value));

        return Result.Success();
    }

    public Result Deactivate(string reason)
    {
        if (!IsActive)
            return Result.Invalid(new ValidationError("Conta já está desativada"));

        IsActive = false;
        DeactivatedAt = DateTime.UtcNow;
        DeactivationReason = reason;

        AddDomainEvent(new UserAccountDeactivatedEvent(Id.Value, reason));

        return Result.Success();
    }

    public Result Reactivate()
    {
        if (IsActive)
            return Result.Invalid(new ValidationError("Conta já está ativa"));

        IsActive = true;
        DeactivatedAt = null;
        DeactivationReason = null;

        AddDomainEvent(new UserAccountReactivatedEvent(Id.Value));
        return Result.Success();
    }
}
