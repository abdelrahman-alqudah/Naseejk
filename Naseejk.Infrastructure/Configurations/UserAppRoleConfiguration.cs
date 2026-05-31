using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naseejk.core.Entities;
namespace Naseejk.Infrastructure.Configurations
{
    public class UserAppRoleConfiguration : IEntityTypeConfiguration<UserAppRole>
    {
        public void Configure(EntityTypeBuilder<UserAppRole> builder)
        {
            builder.ToTable("UserAppRoles");
            builder.HasKey(uar => uar.Id);
            builder.Property(uar => uar.Id).ValueGeneratedNever();
            builder.Property(uar => uar.AssignedByUserId).IsRequired(false);
            builder.Property(uar => uar.AssignAtUtc)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(uar => uar.IsActive)
                .HasDefaultValue(true)
                .IsRequired();
            builder.HasOne(uar => uar.User)
                .WithMany(u => u.UserAppRoles)
                .HasForeignKey(uar => uar.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(uar => uar.Role)
                .WithMany(r => r.UserAppRoles)
                .HasForeignKey(uar => uar.RoleId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(uar => uar.Application)
                .WithMany(a => a.UserAppRoles)
                .HasForeignKey(uar => uar.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
