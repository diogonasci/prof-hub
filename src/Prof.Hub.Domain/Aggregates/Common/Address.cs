using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Common;
public sealed record Address(string Street, string City, string State, string PostalCode)
{
    public static Result<Address> Create(string street, string city, string state, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            return Result.Invalid(new ValidationError("A rua não pode ser vazia."));
        if (string.IsNullOrWhiteSpace(city))
            return Result.Invalid(new ValidationError("A cidade não pode ser vazia."));
        if (string.IsNullOrWhiteSpace(state))
            return Result.Invalid(new ValidationError("O estado não pode ser vazio."));
        if (string.IsNullOrWhiteSpace(postalCode))
            return Result.Invalid(new ValidationError("O CEP não pode ser vazio."));

        return Result.Success(new Address(street, city, state, postalCode));
    }
}
