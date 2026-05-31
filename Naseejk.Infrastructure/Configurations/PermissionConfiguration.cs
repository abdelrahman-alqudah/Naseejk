using System;
using Microsoft.EntityFrameworkCore;
using Naseejk.core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Naseejk.Infrastructure.Configurations
{
    internal class PermissionConfiguration: IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedNever();

            builder.Property(p => p.Key)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(p => new { p.ApplicationId, p.Key }).IsUnique();

            builder.Property(p => p.ApplicationId)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnType("nvarchar(max)");

            builder.HasMany(p => p.RolePermissions)
                .WithOne(rp => rp.Permission)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Application)
                .WithMany(a => a.Permissions)
                .HasForeignKey(p => p.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
