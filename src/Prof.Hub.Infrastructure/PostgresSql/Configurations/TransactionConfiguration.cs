using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.Transaction;
using Prof.Hub.Domain.Aggregates.Transaction.Entities;
using Prof.Hub.Domain.Aggregates.Transaction.ValueObjects;
using Prof.Hub.Domain.Aggregates.Wallet.ValueObjects;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions", tb =>
        {
            tb.HasCheckConstraint("CK_Transaction_Amount_Positive", "\"Amount\" > 0");
        });

        builder.HasKey(t => t.Id);

        // Conversão do Value Object TransactionId
        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                value => new TransactionId(value));

        // WalletId (owned)
        builder.Property(t => t.WalletId)
            .HasConversion(
                id => id.Value,
                value => new WalletId(value))
            .IsRequired();

        // Amount (owned Money)
        builder.OwnsOne(t => t.Amount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL")
                .IsRequired();
        });

        // Propriedades básicas
        builder.Property(t => t.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(t => t.Source)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.ExternalReference)
            .HasMaxLength(100);

        // TransactionDetail (owned collection)
        builder.OwnsMany<TransactionDetail>("_details", detail =>
        {
            detail.ToTable("TransactionDetails");
            detail.WithOwner().HasForeignKey("TransactionId");
            detail.Property<int>("Id").ValueGeneratedOnAdd();
            detail.HasKey("Id");

            detail.Property(d => d.Id)
                .HasConversion(
                    id => id.Value,
                    value => new TransactionDetailId(value));

            detail.Property(d => d.Type)
                .HasConversion<string>()
                .IsRequired();

            detail.Property(d => d.Description)
                .HasMaxLength(200)
                .IsRequired();

            detail.OwnsOne(d => d.Amount, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("Amount")
                    .HasPrecision(18, 2)
                    .IsRequired();

                money.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .HasDefaultValue("BRL")
                    .IsRequired();
            });

            detail.Property(d => d.Reference)
                .HasMaxLength(100);

            detail.Property(d => d.CreatedAt)
                .IsRequired();
        });

        // TransactionReceipt (owned entity)
        builder.OwnsMany<TransactionReceipt>("_receipts", receipt =>
        {
            receipt.ToTable("TransactionReceipts");
            receipt.WithOwner().HasForeignKey("TransactionId");
            receipt.Property<int>("Id").ValueGeneratedOnAdd();
            receipt.HasKey("Id");

            receipt.Property(r => r.Id)
                .HasConversion(
                    id => id.Value,
                    value => new ReceiptId(value));

            receipt.Property(r => r.ReceiptNumber)
                .HasMaxLength(50)
                .IsRequired();

            receipt.Property(r => r.IssuedAt)
                .IsRequired();

            receipt.OwnsOne(r => r.Amount, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("Amount")
                    .HasPrecision(18, 2)
                    .IsRequired();

                money.Property(m => m.Currency)
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .HasDefaultValue("BRL")
                    .IsRequired();
            });

            receipt.OwnsOne(r => r.Taxes, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("TaxAmount")
                    .HasPrecision(18, 2);

                money.Property(m => m.Currency)
                    .HasColumnName("TaxCurrency")
                    .HasMaxLength(3)
                    .HasDefaultValue("BRL");
            });

            receipt.Property(r => r.PayerName)
                .HasMaxLength(200)
                .IsRequired();

            receipt.Property(r => r.PayerDocument)
                .HasMaxLength(20)
                .IsRequired();

            receipt.Property(r => r.BeneficiaryName)
                .HasMaxLength(200)
                .IsRequired();

            receipt.Property(r => r.BeneficiaryDocument)
                .HasMaxLength(20)
                .IsRequired();

            receipt.Property(r => r.Description)
                .HasMaxLength(500)
                .IsRequired();
        });

        // TransactionStatusHistory (owned collection)
        builder.OwnsMany<TransactionStatusHistory>("_statusHistory", history =>
        {
            history.ToTable("TransactionStatusHistory");
            history.WithOwner().HasForeignKey("TransactionId");
            history.Property<int>("Id").ValueGeneratedOnAdd();
            history.HasKey("Id");

            history.Property(h => h.Id)
                .HasConversion(
                    id => id.Value,
                    value => new TransactionStatusHistoryId(value));

            history.Property(h => h.OldStatus)
                .HasConversion<string>()
                .IsRequired();

            history.Property(h => h.NewStatus)
                .HasConversion<string>()
                .IsRequired();

            history.Property(h => h.Reason)
                .HasMaxLength(500);

            history.Property(h => h.ChangedAt)
                .IsRequired();
        });

        // Propriedades de auditoria
        builder.Property(t => t.Created);
        builder.Property(t => t.CreatedBy)
            .HasMaxLength(100);
        builder.Property(t => t.LastModified);
        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(100);

        // Índices
        builder.HasIndex(t => t.WalletId);
        builder.HasIndex(t => t.ExternalReference)
            .HasFilter("\"ExternalReference\" IS NOT NULL");
    }
}
