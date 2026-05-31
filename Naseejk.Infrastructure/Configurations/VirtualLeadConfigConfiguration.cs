using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naseejk.core.Entities;

namespace Naseejk.Infrastructure.Configurations
{
    public class VirtualLeadConfigConfiguration : IEntityTypeConfiguration<VirtualLeadConfig>
    {
        public void Configure(EntityTypeBuilder<VirtualLeadConfig> builder)
        {
            builder.ToTable("VirtualLeadConfigs");

            builder.HasKey(vlc => vlc.Id);

            builder.Property(vlc => vlc.Id).ValueGeneratedNever();

            builder.Property(vlc => vlc.Reason)
                .HasColumnType("nvarchar(max)");

            builder.Property(vlc => vlc.StartAtUtc)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(vlc => vlc.EndsAtUtc)
                .HasColumnType("datetime2")
                .HasDefaultValue(DateTime.MaxValue);

            builder.HasOne(vlc => vlc.Application)
                .WithMany(a => a.VirtualLeadConfigs)
                .HasForeignKey(vlc => vlc.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(vlc => vlc.TargetRole)
                .WithMany(tr => tr.VirtualLeadConfigs)
                .HasForeignKey(vlc => vlc.TargetRoleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(vlc => vlc.PrimaryUser)
                .WithMany(pu => pu.VirtualLeadConfigsAsPrimary)
                .HasForeignKey(vlc => vlc.PrimaryUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(vlc => vlc.SubstituteUser)
                .WithMany(su => su.VirtualLeadConfigsAsSubstitute)
                .HasForeignKey(vlc => vlc.SubstituteUserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
