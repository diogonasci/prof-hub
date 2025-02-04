using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Payment.ValueObjects;
public sealed record CardInfo
{
    public string Brand { get; }
    public string LastDigits { get; }
    public string ExpiryMonth { get; }
    public string ExpiryYear { get; }
    public string TokenizedCard { get; }

    private CardInfo(string brand, string lastDigits, string expiryMonth, string expiryYear, string tokenizedCard)
    {
        Brand = brand;
        LastDigits = lastDigits;
        ExpiryMonth = expiryMonth;
        ExpiryYear = expiryYear;
        TokenizedCard = tokenizedCard;
    }

    public static Result<CardInfo> Create(string brand, string lastDigits, string expiryMonth, string expiryYear, string tokenizedCard)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(brand))
            errors.Add(new ValidationError("A bandeira do cartão é obrigatória"));

        if (string.IsNullOrWhiteSpace(lastDigits) || lastDigits.Length != 4)
            errors.Add(new ValidationError("Os últimos 4 dígitos do cartão são obrigatórios"));

        if (string.IsNullOrWhiteSpace(expiryMonth) || !int.TryParse(expiryMonth, out int month) || month < 1 || month > 12)
            errors.Add(new ValidationError("Mês de expiração do cartão inválido"));

        if (string.IsNullOrWhiteSpace(expiryYear) || !int.TryParse(expiryYear, out int year))
            errors.Add(new ValidationError("Ano de expiração do cartão inválido"));

        if (string.IsNullOrWhiteSpace(tokenizedCard))
            errors.Add(new ValidationError("Token do cartão é obrigatório"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        return new CardInfo(brand, lastDigits, expiryMonth, expiryYear, tokenizedCard);
    }
}
