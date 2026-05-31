using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naseejk.core.Entities;

namespace Naseejk.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.IdentityId)
                .IsRequired()
                .HasMaxLength(450);
            builder.Property(u => u.Id)
                .ValueGeneratedNever();

            builder.HasIndex(u => u.IdentityId).IsUnique();

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(u => u.UserStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.ToTable(t => t.HasCheckConstraint("CK_User_Status", "[UserStatus] IN ('Active', 'Inactive', 'Suspended', 'Pending')"));

            builder.HasMany(u => u.UserExternalIdentities)
                .WithOne(uei => uei.User)
                .HasForeignKey(uei => uei.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserAppRoles)
                .WithOne(uar => uar.User)
                .HasForeignKey(uar => uar.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.TokenHistories)
                .WithOne(th => th.User)
                .HasForeignKey(th => th.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.VirtualLeadConfigsAsPrimary)
                .WithOne(vlc => vlc.PrimaryUser)
                .HasForeignKey(vlc => vlc.PrimaryUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.VirtualLeadConfigsAsSubstitute)
                .WithOne(vlc => vlc.SubstituteUser)
                .HasForeignKey(vlc => vlc.SubstituteUserId)
                .OnDelete(DeleteBehavior.NoAction); 

            builder.HasIndex(u => u.UserName).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}