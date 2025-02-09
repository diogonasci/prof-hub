using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.ReferralCode.Entities;
using Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
using Prof.Hub.Domain.Aggregates.ReferralProgram;
using Prof.Hub.Domain.Aggregates.ReferralProgram.ValueObjects;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
internal sealed class ReferralProgramConfiguration : IEntityTypeConfiguration<ReferralProgram>
{
    public void Configure(EntityTypeBuilder<ReferralProgram> builder)
    {
        // Tabela e chave primária
        builder.ToTable("ReferralPrograms");
        builder.HasKey(r => r.Id);

        // Conversão do Value Object ReferralProgramId
        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => new ReferralProgramId(value));

        // ReferrerReward (owned RewardAmount)
        builder.OwnsOne(r => r.ReferrerReward, reward =>
        {
            reward.Property(r => r.Value)
                .HasColumnName("ReferrerRewardAmount")
                .HasPrecision(18, 2)
                .IsRequired();
        });

        // ReferredReward (owned RewardAmount)
        builder.OwnsOne(r => r.ReferredReward, reward =>
        {
            reward.Property(r => r.Value)
                .HasColumnName("ReferredRewardAmount")
                .HasPrecision(18, 2)
                .IsRequired();
        });

        // CodeValidityPeriod (owned ExpirationPeriod)
        builder.OwnsOne(r => r.CodeValidityPeriod, period =>
        {
            period.Property(p => p.Value)
                .HasColumnName("CodeValidityPeriod")
                .IsRequired();
        });

        // Currency (owned Money)
        builder.OwnsOne(r => r.Currency, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("CurrencyAmount")
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("CurrencyCode")
                .HasMaxLength(3)
                .HasDefaultValue("BRL")
                .IsRequired();
        });

        // Status
        builder.Property(r => r.IsActive)
            .HasDefaultValue(true);

        // ReferralCodes (coleção de relacionamento)
        builder.HasMany(r => r.ReferralCodes)
            .WithOne()
            .HasForeignKey("ReferralProgramId")
            .OnDelete(DeleteBehavior.Restrict);

        // RewardHistory (owned collection)
        builder.OwnsMany(r => r.RewardHistoryList, history =>
        {
            history.ToTable("ReferralProgramRewardHistory");
            history.WithOwner().HasForeignKey("ReferralProgramId");
            history.Property<int>("Id").ValueGeneratedOnAdd();
            history.HasKey("Id");

            // ReferrerReward (owned RewardAmount)
            history.OwnsOne(h => h.ReferrerReward, reward =>
            {
                reward.Property(r => r.Value)
                    .HasColumnName("ReferrerRewardAmount")
                    .HasPrecision(18, 2)
                    .IsRequired();
            });

            // ReferredReward (owned RewardAmount)
            history.OwnsOne(h => h.ReferredReward, reward =>
            {
                reward.Property(r => r.Value)
                    .HasColumnName("ReferredRewardAmount")
                    .HasPrecision(18, 2)
                    .IsRequired();
            });

            history.Property(h => h.UpdatedAt)
                .IsRequired();

            // Índice para consultas históricas
            history.HasIndex(h => h.UpdatedAt);
        });

        // Propriedades de auditoria
        builder.Property(r => r.Created);
        builder.Property(r => r.CreatedBy)
            .HasMaxLength(100);
        builder.Property(r => r.LastModified);
        builder.Property(r => r.LastModifiedBy)
            .HasMaxLength(100);

        // Check Constraints
        builder.ToTable(tb => tb.HasCheckConstraint(
            "CK_ReferralProgram_RewardAmounts_Positive",
            """
            "ReferrerRewardAmount" > 0 AND 
            "ReferredRewardAmount" > 0
            """));

        // Índices
        builder.HasIndex(r => r.IsActive);
        builder.HasIndex(r => r.Created);
    }
}

// Configuração complementar para ReferralReward
internal sealed class ReferralRewardConfiguration : IEntityTypeConfiguration<ReferralReward>
{
    public void Configure(EntityTypeBuilder<ReferralReward> builder)
    {
        builder.ToTable("ReferralRewards");
        builder.HasKey(r => r.Id);

        // Conversão do Value Object ReferralRewardId
        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => new ReferralRewardId(value));

        // ReferralInviteId (owned)
        builder.Property(r => r.ReferralInviteId)
            .HasConversion(
                id => id.Value,
                value => new ReferralInviteId(value))
            .IsRequired();

        // StudentId (owned)
        builder.Property(r => r.StudentId)
            .HasConversion(
                id => id.Value,
                value => new StudentId(value))
            .IsRequired();

        // Amount (owned RewardAmount)
        builder.OwnsOne(r => r.Amount, amount =>
        {
            amount.Property(a => a.Value)
                .HasColumnName("RewardAmount")
                .HasPrecision(18, 2)
                .IsRequired();
        });

        // Status
        builder.Property(r => r.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(r => r.ProcessedAt);

        // Propriedades de auditoria
        builder.Property(r => r.Created);
        builder.Property(r => r.CreatedBy)
            .HasMaxLength(100);
        builder.Property(r => r.LastModified);
        builder.Property(r => r.LastModifiedBy)
            .HasMaxLength(100);

        // Check Constraints
        builder.ToTable(tb => tb.HasCheckConstraint(
            "CK_ReferralReward_Amount_Positive",
            "\"RewardAmount\" > 0"));

        // Índices
        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => r.StudentId);
        builder.HasIndex(r => r.ProcessedAt);
    }
}
