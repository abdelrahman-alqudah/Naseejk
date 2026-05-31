using Microsoft.EntityFrameworkCore;
using Naseejk.core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Naseejk.Infrastructure.Configurations
{
    public class UserExternalIdentitieConfiguration:IEntityTypeConfiguration<UserExternalIdentitie>
    {
        public void Configure(EntityTypeBuilder<UserExternalIdentitie> builder)
        {
            builder.ToTable("UserExternalIdentities");
            builder.HasKey(uei => uei.Id);
            builder.Property(uei => uei.Id).ValueGeneratedNever();
            builder.Property(uei => uei.IdentitySource)
                .IsRequired()
                .HasMaxLength(256);
            builder.Property(uei => uei.IsVerified)
                .IsRequired();
            builder.Property(uei => uei.CreatedAtUtc)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");
            builder.HasOne(uei => uei.User)
                .WithMany(u => u.UserExternalIdentities)
                .HasForeignKey(uei => uei.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(uei => uei.Application)
                .WithMany(a => a.UserExternalIdentities)
                .HasForeignKey(uei => uei.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
