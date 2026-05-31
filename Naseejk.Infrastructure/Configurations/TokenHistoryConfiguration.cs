using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naseejk.core.Entities;

namespace Naseejk.Infrastructure.Configurations
{
    public class TokenHistoryConfiguration : IEntityTypeConfiguration<TokenHistory>
    {
        public void Configure(EntityTypeBuilder<TokenHistory> builder)
        {
            builder.ToTable("TokenHistory");

            builder.HasKey(th => th.Id);

            builder.Property(th => th.Id).ValueGeneratedNever();

            builder.Property(th => th.TokenHash)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(th => th.IssuedAtUtc)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(th => th.ExpiresAtUtc)
                .HasColumnType("datetime2");

            builder.Property(th => th.RevokedAtUtc)
                .HasColumnType("datetime2");

            builder.Property(th => th.IpAddress)
                .HasMaxLength(256);

            builder.Property(th => th.CorrelationId)
                .HasMaxLength(256);

            builder.Property(th => th.TraceId)
                .HasMaxLength(256);

            builder.HasOne(th => th.User)
                .WithMany(u => u.TokenHistories)
                .HasForeignKey(th => th.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(th => th.Application)
                .WithMany(a => a.TokenHistories)
                .HasForeignKey(th => th.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(th => th.TokenHash);
        }

    }
}