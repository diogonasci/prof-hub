using Prof.Hub.Domain.Aggregates.Common.ValueObjects;
using Prof.Hub.Domain.Aggregates.Transaction.ValueObjects;
using Prof.Hub.SharedKernel;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Domain.Aggregates.Transaction.Entities;
public class TransactionReceipt : Entity
{
    public ReceiptId Id { get; private set; }
    public TransactionId TransactionId { get; private set; }
    public string ReceiptNumber { get; private set; }
    public DateTime IssuedAt { get; private set; }
    public Money Amount { get; private set; }
    public Money? Taxes { get; private set; }
    public string PayerName { get; private set; }
    public string PayerDocument { get; private set; }
    public string BeneficiaryName { get; private set; }
    public string BeneficiaryDocument { get; private set; }
    public string Description { get; private set; }

    private TransactionReceipt() { }

    public static Result<TransactionReceipt> Create(
        TransactionId transactionId,
        Money amount,
        string payerName,
        string payerDocument,
        string beneficiaryName,
        string beneficiaryDocument,
        string description,
        Money? taxes = null)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(payerName))
            errors.Add(new ValidationError("Nome do pagador é obrigatório"));

        if (string.IsNullOrWhiteSpace(payerDocument))
            errors.Add(new ValidationError("Documento do pagador é obrigatório"));

        if (string.IsNullOrWhiteSpace(beneficiaryName))
            errors.Add(new ValidationError("Nome do beneficiário é obrigatório"));

        if (string.IsNullOrWhiteSpace(beneficiaryDocument))
            errors.Add(new ValidationError("Documento do beneficiário é obrigatório"));

        if (errors.Count > 0)
            return Result.Invalid(errors);

        var receipt = new TransactionReceipt
        {
            Id = ReceiptId.Create(),
            TransactionId = transactionId,
            ReceiptNumber = GenerateReceiptNumber(),
            IssuedAt = DateTime.UtcNow,
            Amount = amount,
            Taxes = taxes,
            PayerName = payerName,
            PayerDocument = payerDocument,
            BeneficiaryName = beneficiaryName,
            BeneficiaryDocument = beneficiaryDocument,
            Description = description
        };

        return receipt;
    }

    private static string GenerateReceiptNumber()
    {
        return $"REC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
    }
}
