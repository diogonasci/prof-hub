using Prof.Hub.Domain.Aggregates.Payment.ValueObjects;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Payment.Entities;
public class BillingAddress
{
    public BillingAddressId Id { get; private set; }
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string? Complement { get; private set; }
    public string Neighborhood { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string PostalCode { get; private set; }
    public bool IsDefault { get; private set; }

    private BillingAddress() { }

    public static Result<BillingAddress> Create(
        string street,
        string number,
        string neighborhood,
        string city,
        string state,
        string postalCode,
        string? complement = null,
        bool isDefault = false)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(street))
            errors.Add(new ValidationError("Rua é obrigatória"));

        if (string.IsNullOrWhiteSpace(number))
            errors.Add(new ValidationError("Número é obrigatório"));

        if (string.IsNullOrWhiteSpace(neighborhood))
            errors.Add(new ValidationError("Bairro é obrigatório"));

        if (string.IsNullOrWhiteSpace(city))
            errors.Add(new ValidationError("Cidade é obrigatória"));

        if (string.IsNullOrWhiteSpace(state))
            errors.Add(new ValidationError("Estado é obrigatório"));

        if (string.IsNullOrWhiteSpace(postalCode))
            errors.Add(new ValidationError("CEP é obrigatório"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var address = new BillingAddress
        {
            Id = BillingAddressId.Create(),
            Street = street,
            Number = number,
            Complement = complement,
            Neighborhood = neighborhood,
            City = city,
            State = state,
            PostalCode = postalCode,
            IsDefault = isDefault
        };

        return address;
    }

    public void SetDefault(bool isDefault)
    {
        IsDefault = isDefault;
    }
}
