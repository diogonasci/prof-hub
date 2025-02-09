using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prof.Hub.Domain.Aggregates.ReferralCode;
using Prof.Hub.Domain.Aggregates.ReferralCode.Entities;
using Prof.Hub.Domain.Aggregates.ReferralCode.ValueObjects;
using Prof.Hub.Domain.Aggregates.ReferralProgram;
using Prof.Hub.Domain.Aggregates.Student.ValueObjects;

namespace Prof.Hub.Infrastructure.PostgresSql.Configurations;
internal sealed class ReferralCodeConfiguration : IEntityTypeConfiguration<ReferralCode>
{
    public void Configure(EntityTypeBuilder<ReferralCode> builder)
    {
        // Tabela e chave primária
        builder.ToTable("ReferralCodes");
        builder.HasKey(r => r.Id);

        // Conversão do Value Object ReferralCodeId
        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => new ReferralCodeId(value));

        // ReferrerId (owned)
        builder.Property(r => r.ReferrerId)
            .HasConversion(
                id => id.Value,
                value => new StudentId(value))
            .IsRequired();

        // Code (owned ReferralCodeValue)
        builder.OwnsOne(r => r.Code, code =>
        {
            code.Property(c => c.Value)
                .HasColumnName("Code")
                .HasMaxLength(8)
                .IsRequired();
        });

        // Propriedades básicas
        builder.Property(r => r.ExpiresAt)
            .IsRequired();
        builder.Property(r => r.IsActive)
            .HasDefaultValue(true);
        builder.Property(r => r.LastUsedAt);

        // ReferralInvites (owned collection)
        builder.OwnsMany<ReferralInvite>("_invites", invite =>
        {
            invite.ToTable("ReferralCodeInvites");
            invite.WithOwner().HasForeignKey("ReferralCodeId");

            // Chave primária
            invite.HasKey(i => i.Id);
            invite.Property(i => i.Id)
                .HasConversion(
                    id => id.Value,
                    value => new ReferralInviteId(value));

            // ReferrerId
            invite.Property(i => i.ReferrerId)
                .HasConversion(
                    id => id.Value,
                    value => new StudentId(value))
                .IsRequired();

            // ReferredEmail
            invite.OwnsOne(i => i.ReferredEmail, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("ReferredEmail")
                    .HasMaxLength(256)
                    .IsRequired();
            });

            // Status
            invite.Property(i => i.Status)
                .HasConversion<string>()
                .IsRequired();

            invite.Property(i => i.CompletedAt);

            // ReferralRewards (owned collection dentro do invite)
            invite.OwnsMany<ReferralReward>("_rewards", reward =>
            {
                reward.ToTable("ReferralInviteRewards");
                reward.WithOwner().HasForeignKey("ReferralInviteId");

                // Chave primária
                reward.HasKey(r => r.Id);
                reward.Property(r => r.Id)
                    .HasConversion(
                        id => id.Value,
                        value => new ReferralRewardId(value));

                // StudentId
                reward.Property(r => r.StudentId)
                    .HasConversion(
                        id => id.Value,
                        value => new StudentId(value))
                    .IsRequired();

                // Amount
                reward.OwnsOne(r => r.Amount, amount =>
                {
                    amount.Property(a => a.Value)
                        .HasColumnName("RewardAmount")
                        .HasPrecision(18, 2)
                        .IsRequired();
                });

                // Status
                reward.Property(r => r.Status)
                    .HasConversion<string>()
                    .IsRequired();

                reward.Property(r => r.ProcessedAt);

                // Auditoria
                reward.Property(r => r.Created);
                reward.Property(r => r.CreatedBy)
                    .HasMaxLength(100);
                reward.Property(r => r.LastModified);
                reward.Property(r => r.LastModifiedBy)
                    .HasMaxLength(100);

                // Índices
                reward.HasIndex(r => r.Status);
                reward.HasIndex(r => r.ProcessedAt);
            });

            // Auditoria do invite
            invite.Property(i => i.Created);
            invite.Property(i => i.CreatedBy)
                .HasMaxLength(100);
            invite.Property(i => i.LastModified);
            invite.Property(i => i.LastModifiedBy)
                .HasMaxLength(100);

            // Índices
            invite.HasIndex(i => i.Status);
            invite.HasIndex(i => i.CompletedAt);
            invite.HasIndex(i => i.ReferredEmail);
        });

        // Check Constraints
        builder.ToTable(tb => tb.HasCheckConstraint(
            "CK_ReferralCode_ExpiresAt_Future",
            "\"ExpiresAt\" > CURRENT_TIMESTAMP"));

        // Propriedades de auditoria
        builder.Property(r => r.Created);
        builder.Property(r => r.CreatedBy)
            .HasMaxLength(100);
        builder.Property(r => r.LastModified);
        builder.Property(r => r.LastModifiedBy)
            .HasMaxLength(100);

        // Índices
        builder.HasIndex(r => r.Code)
            .IsUnique();
        builder.HasIndex(r => r.ReferrerId);
        builder.HasIndex(r => r.ExpiresAt);
        builder.HasIndex(r => r.IsActive);
        builder.HasIndex(r => r.LastUsedAt);

        // Relacionamentos
        builder.HasOne<ReferralProgram>()
            .WithMany(p => p.ReferralCodes)
            .HasForeignKey("ReferralProgramId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}
