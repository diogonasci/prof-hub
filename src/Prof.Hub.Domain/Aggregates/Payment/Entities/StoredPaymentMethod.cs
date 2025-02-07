using Prof.Hub.Domain.Aggregates.Payment.ValueObjects;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Payment.Entities;
public class StoredPaymentMethod
{
    public StoredPaymentMethodId Id { get; private set; }
    public CardInfo CardInfo { get; private set; }
    public bool IsDefault { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastUsedAt { get; private set; }

    private StoredPaymentMethod()
    {
        IsActive = true;
    }

    public static Result<StoredPaymentMethod> Create(
        string cardNumber,
        string cardHolderName,
        string expiryMonth,
        string expiryYear,
        string cvv,
        bool isDefault = false)
    {
        var cardInfoResult = CardInfo.Create(
            cardNumber,
            cardHolderName,
            expiryMonth,
            expiryYear,
            cvv);

        if (!cardInfoResult.IsSuccess)
            return Result.Invalid(cardInfoResult.ValidationErrors);

        var method = new StoredPaymentMethod
        {
            Id = StoredPaymentMethodId.Create(),
            CardInfo = cardInfoResult.Value,
            IsDefault = isDefault
        };

        return method;
    }

    public void SetDefault(bool isDefault)
    {
        IsDefault = isDefault;
    }

    public void RegisterUsage()
    {
        LastUsedAt = DateTime.UtcNow;
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return Result.Invalid(new ValidationError("Método de pagamento já está inativo"));

        if (IsDefault)
            return Result.Invalid(new ValidationError("Não é possível desativar o método de pagamento padrão"));

        IsActive = false;
        return Result.Success();
    }
}
