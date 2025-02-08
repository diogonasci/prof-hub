using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Payment.Entities;
using Prof.Hub.Domain.Aggregates.Payment.Events;
using Prof.Hub.Domain.Aggregates.Payment.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Enums;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Payment;
public class Payment : AuditableEntity, IAggregateRoot
{
    private readonly List<StoredPaymentMethod> _storedPaymentMethods = [];
    private readonly List<BillingAddress> _billingAddresses = [];

    private const int MAX_STORED_METHODS = 5;
    private const int MAX_BILLING_ADDRESSES = 3;

    public PaymentId Id { get; private set; }
    public StudentId StudentId { get; private set; }
    public Money Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public StoredPaymentMethod? UsedPaymentMethod { get; private set; }
    public BillingAddress? UsedBillingAddress { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public IReadOnlyList<StoredPaymentMethod> StoredPaymentMethods => _storedPaymentMethods.AsReadOnly();
    public IReadOnlyList<BillingAddress> BillingAddresses => _billingAddresses.AsReadOnly();

    private Payment()
    {
        CreatedAt = DateTime.UtcNow;
        Status = PaymentStatus.Pending;
    }

    // Factory method para criar um novo pagamento
    public static Result<Payment> Create(StudentId studentId, Money amount, StoredPaymentMethod? paymentMethod = null, BillingAddress? billingAddress = null)
    {
        if (amount.Amount <= 0)
            return Result.Invalid(new ValidationError("Valor do pagamento deve ser maior que zero"));

        var payment = new Payment
        {
            Id = PaymentId.Create(),
            StudentId = studentId,
            Amount = amount,
            UsedPaymentMethod = paymentMethod,
            UsedBillingAddress = billingAddress
        };

        payment.AddDomainEvent(new PaymentCreatedEvent(payment.Id.Value, studentId.Value));
        return payment;
    }

    // Métodos para processar o pagamento
    public Result Process()
    {
        if (Status != PaymentStatus.Pending)
            return Result.Invalid(new ValidationError("Pagamento já foi processado"));

        if (UsedPaymentMethod == null)
            return Result.Invalid(new ValidationError("Método de pagamento não informado"));

        Status = PaymentStatus.Processing;
        AddDomainEvent(new PaymentProcessingEvent(Id.Value));

        return Result.Success();
    }

    public Result Complete()
    {
        if (Status != PaymentStatus.Processing)
            return Result.Invalid(new ValidationError("Pagamento deve estar em processamento para ser concluído"));

        Status = PaymentStatus.Completed;
        CompletedAt = DateTime.UtcNow;

        if (UsedPaymentMethod != null)
            UsedPaymentMethod.RegisterUsage();

        AddDomainEvent(new PaymentCompletedEvent(Id.Value));
        return Result.Success();
    }

    public Result Fail()
    {
        if (Status != PaymentStatus.Processing)
            return Result.Invalid(new ValidationError("Pagamento deve estar em processamento para ser marcado como falho"));

        Status = PaymentStatus.Failed;
        AddDomainEvent(new PaymentFailedEvent(Id.Value));

        return Result.Success();
    }

    // Métodos para gerenciar cartões salvos
    public Result AddStoredPaymentMethod(StoredPaymentMethod paymentMethod)
    {
        if (_storedPaymentMethods.Count >= MAX_STORED_METHODS)
            return Result.Invalid(new ValidationError($"Máximo de {MAX_STORED_METHODS} métodos de pagamento permitidos"));

        if (_storedPaymentMethods.Any(m => m.IsDefault && paymentMethod.IsDefault))
        {
            var defaultMethod = _storedPaymentMethods.First(m => m.IsDefault);
            defaultMethod.SetDefault(false);
        }

        _storedPaymentMethods.Add(paymentMethod);
        AddDomainEvent(new StoredPaymentMethodAddedEvent(Id.Value, StudentId.Value, paymentMethod.Id.Value));

        return Result.Success();
    }

    public Result RemoveStoredPaymentMethod(StoredPaymentMethodId methodId)
    {
        var method = _storedPaymentMethods.FirstOrDefault(m => m.Id == methodId);
        if (method == null)
            return Result.Invalid(new ValidationError("Método de pagamento não encontrado"));

        if (method.IsDefault)
            return Result.Invalid(new ValidationError("Não é possível remover o método de pagamento padrão"));

        _storedPaymentMethods.Remove(method);
        AddDomainEvent(new StoredPaymentMethodRemovedEvent(Id.Value, StudentId.Value, methodId.Value));

        return Result.Success();
    }

    // Métodos para gerenciar endereços de cobrança
    public Result AddBillingAddress(BillingAddress address)
    {
        if (_billingAddresses.Count >= MAX_BILLING_ADDRESSES)
            return Result.Invalid(new ValidationError($"Máximo de {MAX_BILLING_ADDRESSES} endereços permitidos"));

        if (_billingAddresses.Any(a => a.IsDefault && address.IsDefault))
        {
            var defaultAddress = _billingAddresses.First(a => a.IsDefault);
            defaultAddress.SetDefault(false);
        }

        _billingAddresses.Add(address);
        AddDomainEvent(new BillingAddressAddedEvent(Id.Value, StudentId.Value, address.Id.Value));

        return Result.Success();
    }

    public Result RemoveBillingAddress(BillingAddressId addressId)
    {
        var address = _billingAddresses.FirstOrDefault(a => a.Id == addressId);
        if (address == null)
            return Result.Invalid(new ValidationError("Endereço não encontrado"));

        if (address.IsDefault)
            return Result.Invalid(new ValidationError("Não é possível remover o endereço padrão"));

        _billingAddresses.Remove(address);
        AddDomainEvent(new BillingAddressRemovedEvent(Id.Value, StudentId.Value, addressId.Value));

        return Result.Success();
    }

    // Métodos auxiliares
    public StoredPaymentMethod? GetDefaultPaymentMethod()
        => _storedPaymentMethods.FirstOrDefault(m => m.IsDefault);

    public BillingAddress? GetDefaultBillingAddress()
        => _billingAddresses.FirstOrDefault(a => a.IsDefault);
}
