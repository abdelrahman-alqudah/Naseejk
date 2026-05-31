using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naseejk.core.Entities;

namespace Naseejk.Infrastructure.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.ToTable("Applications");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id).ValueGeneratedNever();

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.HasIndex(a => a.ClientIdentifier)
                .IsUnique();

            builder.Property(a => a.ClientIdentifier)
                .IsRequired();

            builder.HasMany(a => a.Roles)
                .WithOne(r => r.Application)
                .HasForeignKey(r => r.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(a => a.Permissions)
                .WithOne(p => p.Application)
                .HasForeignKey(p => p.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(a => a.UserExternalIdentities)
                .WithOne(uei => uei.Application)
                .HasForeignKey(uei => uei.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(a => a.AuditLogs)
                .WithOne(al => al.Application)
                .HasForeignKey(al => al.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(a => a.UserAppRoles)
                .WithOne(uar => uar.Application)
                .HasForeignKey(uar => uar.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(a => a.TokenHistories)
                .WithOne(th => th.Application)
                .HasForeignKey(th => th.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(a => a.VirtualLeadConfigs)
                .WithOne(vlc => vlc.Application)
                .HasForeignKey(vlc => vlc.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(a => a.Tenant)
                .WithMany(t => t.Applications)
                .HasForeignKey(a => a.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(a => new { a.TenantId, a.Code }).IsUnique();
        }
    }
}
