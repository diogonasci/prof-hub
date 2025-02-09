using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;
using Prof.Hub.Domain.Aggregates.Wallet;
using Prof.Hub.Domain.Aggregates.Wallet.ValueObjects;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
internal sealed class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("Wallets", tb =>
        {
            tb.HasCheckConstraint("CK_Wallet_Balance_NonNegative", "\"Balance\" >= 0");
            tb.HasCheckConstraint("CK_Wallet_DailyLimit_Positive", "\"DailyLimit\" > 0 OR \"DailyLimit\" IS NULL");
            tb.HasCheckConstraint("CK_Wallet_MonthlyLimit_Positive", "\"MonthlyLimit\" > 0 OR \"MonthlyLimit\" IS NULL");
            tb.HasCheckConstraint("CK_Wallet_MonthlyLimit_GreaterThanDaily",
                "\"MonthlyLimit\" > \"DailyLimit\" OR \"MonthlyLimit\" IS NULL OR \"DailyLimit\" IS NULL");
        });

        builder.HasKey(w => w.Id);

        // Conversão do Value Object WalletId
        builder.Property(w => w.Id)
            .HasConversion(
                id => id.Value,
                value => new WalletId(value));

        // StudentId (owned)
        builder.Property(w => w.StudentId)
            .HasConversion(
                id => id.Value,
                value => new StudentId(value))
            .IsRequired();

        // Balance (owned Money)
        builder.OwnsOne(w => w.Balance, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Balance")
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL")
                .IsRequired();
        });

        // DailyLimit (owned Money nullable)
        builder.OwnsOne(w => w.DailyLimit, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("DailyLimit")
                .HasPrecision(18, 2);

            money.Property(m => m.Currency)
                .HasColumnName("DailyLimitCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        // MonthlyLimit (owned Money nullable)
        builder.OwnsOne(w => w.MonthlyLimit, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("MonthlyLimit")
                .HasPrecision(18, 2);

            money.Property(m => m.Currency)
                .HasColumnName("MonthlyLimitCurrency")
                .HasMaxLength(3)
                .HasDefaultValue("BRL");
        });

        // Última transação
        builder.Property(w => w.LastTransactionDate);

        // Transactions navigation
        builder.HasMany(w => w.Transactions)
            .WithOne()
            .HasForeignKey("WalletId")
            .OnDelete(DeleteBehavior.Restrict);

        // Propriedades de auditoria
        builder.Property(w => w.Created);
        builder.Property(w => w.CreatedBy)
            .HasMaxLength(100);
        builder.Property(w => w.LastModified);
        builder.Property(w => w.LastModifiedBy)
            .HasMaxLength(100);

        // Índices
        builder.HasIndex(w => w.StudentId)
            .IsUnique();
    }
}
