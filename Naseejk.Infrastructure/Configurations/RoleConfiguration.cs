using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naseejk.core.Entities;

namespace Naseejk.Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .ValueGeneratedNever();

            builder.Property(r => r.ApplicationId)
                .IsRequired();

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.NormalizedName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.IsSystemRole)
                .IsRequired();

            builder.HasOne(r => r.Application)
                .WithMany(a => a.Roles)
                .HasForeignKey(r => r.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.RolePermissions)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.UserAppRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.VirtualLeadConfigs)
                .WithOne(vlc => vlc.TargetRole)
                .HasForeignKey(vlc => vlc.TargetRoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(r => new { r.ApplicationId, r.Name }).IsUnique();
            builder.HasIndex(r => new { r.ApplicationId, r.NormalizedName }).IsUnique();
        }
    }
}
