using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.Payment;
using Prof.Hub.Domain.Aggregates.Payment.Entities;
using Prof.Hub.Domain.Aggregates.Payment.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        // Conversão do Value Object PaymentId
        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => new PaymentId(value));

        // StudentId (owned)
        builder.Property(p => p.StudentId)
            .HasConversion(
                id => id.Value,
                value => new StudentId(value))
            .IsRequired();

        // Amount (owned Money)
        builder.OwnsOne(p => p.Amount, money =>
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

        // Status
        builder.Property(p => p.Status)
            .HasConversion<string>()
            .IsRequired();

        // Datas
        builder.Property(p => p.CreatedAt);
        builder.Property(p => p.CompletedAt);

        // StoredPaymentMethods (owned collection)
        builder.OwnsMany<StoredPaymentMethod>("_storedPaymentMethods", method =>
        {
            method.ToTable("StoredPaymentMethods");
            method.WithOwner().HasForeignKey("PaymentId");

            // Chave primária
            method.HasKey(m => m.Id);
            method.Property(m => m.Id)
                .HasConversion(
                    id => id.Value,
                    value => new StoredPaymentMethodId(value));

            // CardInfo (owned)
            method.OwnsOne(m => m.CardInfo, card =>
            {
                card.Property(c => c.Number)
                    .HasMaxLength(19)
                    .IsRequired();

                card.Property(c => c.HolderName)
                    .HasMaxLength(100)
                    .IsRequired();

                card.Property(c => c.ExpiryMonth)
                    .HasMaxLength(2)
                    .IsRequired();

                card.Property(c => c.ExpiryYear)
                    .HasMaxLength(4)
                    .IsRequired();

                card.Property(c => c.Cvv)
                    .HasMaxLength(4)
                    .IsRequired();

                card.Property(c => c.Brand)
                    .HasMaxLength(20)
                    .IsRequired();

                card.Property(c => c.LastFourDigits)
                    .HasMaxLength(4)
                    .IsRequired();
            });

            method.Property(m => m.IsDefault)
                .HasDefaultValue(false);

            method.Property(m => m.IsActive)
                .HasDefaultValue(true);

            method.Property(m => m.LastUsedAt);

            // Índices
            method.HasIndex(m => m.IsDefault);
            method.HasIndex(m => m.IsActive);
            method.HasIndex(m => m.LastUsedAt);
        });

        // BillingAddresses (owned collection)
        builder.OwnsMany<BillingAddress>("_billingAddresses", address =>
        {
            address.ToTable("BillingAddresses");
            address.WithOwner().HasForeignKey("PaymentId");

            // Chave primária
            address.HasKey(a => a.Id);
            address.Property(a => a.Id)
                .HasConversion(
                    id => id.Value,
                    value => new BillingAddressId(value));

            // Propriedades do endereço
            address.Property(a => a.Street)
                .HasMaxLength(200)
                .IsRequired();

            address.Property(a => a.Number)
                .HasMaxLength(20)
                .IsRequired();

            address.Property(a => a.Complement)
                .HasMaxLength(100);

            address.Property(a => a.Neighborhood)
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.City)
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.State)
                .HasMaxLength(2)
                .IsRequired();

            address.Property(a => a.PostalCode)
                .HasMaxLength(8)
                .IsRequired();

            address.Property(a => a.IsDefault)
                .HasDefaultValue(false);

            // Índices
            address.HasIndex(a => a.PostalCode);
            address.HasIndex(a => a.IsDefault);
        });

        // Propriedades de auditoria
        builder.Property(p => p.Created);
        builder.Property(p => p.CreatedBy)
            .HasMaxLength(100);
        builder.Property(p => p.LastModified);
        builder.Property(p => p.LastModifiedBy)
            .HasMaxLength(100);

        // Índices
        builder.HasIndex(p => p.StudentId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.CreatedAt);
        builder.HasIndex(p => p.CompletedAt);



        builder.Property<string>("UsedPaymentMethodId");
        builder.Property<string>("UsedBillingAddressId");
    }
}
