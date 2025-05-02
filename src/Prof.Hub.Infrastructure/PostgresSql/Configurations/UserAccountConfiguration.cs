using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.UserAccount;
using Prof.Hub.Domain.Aggregates.UserAccount.ValueObjects;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
internal sealed class UserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.ToTable("UserAccounts");

        builder.HasKey(u => u.Id);

        // Conversão do Value Object UserAccountId
        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => new UserAccountId(value));

        // Email (owned entity)
        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(256);
        });

        // PhoneNumber (owned entity)
        builder.OwnsOne(u => u.PhoneNumber, phone =>
        {
            phone.Property(p => p.Value)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(20);
        });

        // Propriedades básicas
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(u => u.AvatarUrl)
            .HasConversion(
                url => url != null ? url.ToString() : null,
                value => value != null ? new Uri(value) : null);

        builder.Property(u => u.IsEmailVerified)
            .HasDefaultValue(false);

        builder.Property(u => u.IsPhoneVerified)
            .HasDefaultValue(false);

        builder.Property(u => u.IsActive)
            .HasDefaultValue(true);

        builder.Property(u => u.DeactivatedAt);

        builder.Property(u => u.DeactivationReason)
            .HasMaxLength(500);

        // LoginProviders (owned collection)
        builder.OwnsMany(u => u.LoginProviders, provider =>
        {
            provider.ToTable("UserLoginProviders");
            provider.WithOwner().HasForeignKey("UserAccountId");
            provider.Property<int>("Id").ValueGeneratedOnAdd();
            provider.HasKey("Id");

            provider.Property(p => p.Type)
                .HasConversion<string>();

            provider.Property(p => p.ExternalId)
                .IsRequired()
                .HasMaxLength(100);

            provider.Property(p => p.DisplayName)
                .IsRequired()
                .HasMaxLength(100);

            provider.Property(p => p.ConnectedAt);

            // Índice único para evitar duplicatas do mesmo provedor
            provider.HasIndex("Type", "UserAccountId").IsUnique();
        });

        // NotificationPreferences (owned collection)
        builder.OwnsMany(u => u.NotificationPreferences, pref =>
        {
            pref.ToTable("UserNotificationPreferences");
            pref.WithOwner().HasForeignKey("UserAccountId");
            pref.Property<int>("Id").ValueGeneratedOnAdd();
            pref.HasKey("Id");

            pref.Property(p => p.Type)
                .HasConversion<string>();

            pref.Property(p => p.IsEnabled)
                .HasDefaultValue(true);

            // Índice único para evitar duplicatas do mesmo tipo
            pref.HasIndex("Type", "UserAccountId").IsUnique();
        });

        // Propriedades de auditoria
        builder.Property(u => u.Created);
        builder.Property(u => u.CreatedBy)
            .HasMaxLength(100);
        builder.Property(u => u.LastModified);
        builder.Property(u => u.LastModifiedBy)
            .HasMaxLength(100);

        // Índices
        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.PhoneNumber)
            .IsUnique()
            .HasFilter("\"PhoneNumber\" IS NOT NULL");
    }
}
