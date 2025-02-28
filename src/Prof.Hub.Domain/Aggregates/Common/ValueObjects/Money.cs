using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common.ValueObjects;
public sealed record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Result<Money> Create(decimal amount, string currency = "BRL")
    {
        var errors = new List<ValidationError>();

        if (amount < 0)
            errors.Add(new ValidationError("O valor não pode ser negativo"));

        if (string.IsNullOrWhiteSpace(currency))
            errors.Add(new ValidationError("A moeda é obrigatória"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        return new Money(amount, currency);
    }

    public static Result<Money> operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            return Result.Invalid(new ValidationError("Não é possível somar valores em moedas diferentes"));

        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Result<Money> operator -(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            return Result.Invalid(new ValidationError("Não é possível subtrair valores em moedas diferentes"));

        return new Money(a.Amount - b.Amount, a.Currency);
    }

    public static bool operator <(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Não é possível comparar valores em moedas diferentes");

        return a.Amount < b.Amount;
    }

    public static bool operator >(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Não é possível comparar valores em moedas diferentes");

        return a.Amount > b.Amount;
    }

    public static bool operator <=(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Não é possível comparar valores em moedas diferentes");

        return a.Amount <= b.Amount;
    }

    public static bool operator >=(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Não é possível comparar valores em moedas diferentes");

        return a.Amount >= b.Amount;
    }
}
