using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Payment.ValueObjects;
public record CardInfo
{
    public string Number { get; }
    public string HolderName { get; }
    public string ExpiryMonth { get; }
    public string ExpiryYear { get; }
    public string Cvv { get; }
    public string Brand { get; }
    public string LastFourDigits { get; }

    private CardInfo(
        string number,
        string holderName,
        string expiryMonth,
        string expiryYear,
        string cvv,
        string brand,
        string lastFourDigits)
    {
        Number = number;
        HolderName = holderName;
        ExpiryMonth = expiryMonth;
        ExpiryYear = expiryYear;
        Cvv = cvv;
        Brand = brand;
        LastFourDigits = lastFourDigits;
    }

    public static Result<CardInfo> Create(
        string number,
        string holderName,
        string expiryMonth,
        string expiryYear,
        string cvv)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(number) || !IsValidCardNumber(number))
            errors.Add(new ValidationError("Número do cartão inválido"));

        if (string.IsNullOrWhiteSpace(holderName))
            errors.Add(new ValidationError("Nome do titular é obrigatório"));

        if (!IsValidExpiryMonth(expiryMonth))
            errors.Add(new ValidationError("Mês de expiração inválido"));

        if (!IsValidExpiryYear(expiryYear))
            errors.Add(new ValidationError("Ano de expiração inválido"));

        if (!IsValidCvv(cvv))
            errors.Add(new ValidationError("CVV inválido"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var brand = DetectCardBrand(number);
        var lastFourDigits = number.Substring(number.Length - 4);

        return new CardInfo(
            number,
            holderName,
            expiryMonth,
            expiryYear,
            cvv,
            brand,
            lastFourDigits);
    }

    private static bool IsValidCardNumber(string number)
    {
        // Implementar validação de número de cartão (Luhn algorithm)
        return number.All(char.IsDigit) && number.Length >= 13 && number.Length <= 19;
    }

    private static bool IsValidExpiryMonth(string month)
    {
        return int.TryParse(month, out int m) && m >= 1 && m <= 12;
    }

    private static bool IsValidExpiryYear(string year)
    {
        if (!int.TryParse(year, out int y))
            return false;

        var currentYear = DateTime.UtcNow.Year;
        return y >= currentYear && y <= currentYear + 20;
    }

    private static bool IsValidCvv(string cvv)
    {
        return cvv.Length >= 3 && cvv.Length <= 4 && cvv.All(char.IsDigit);
    }

    private static string DetectCardBrand(string number)
    {
        if (string.IsNullOrEmpty(number)) return "Unknown";

        // Lógica simplificada de detecção de bandeira
        return number[0] switch
        {
            '4' => "Visa",
            '5' => "Mastercard",
            '3' => number.Length == 15 ? "Amex" : "Unknown",
            _ => "Unknown"
        };
    }
}
