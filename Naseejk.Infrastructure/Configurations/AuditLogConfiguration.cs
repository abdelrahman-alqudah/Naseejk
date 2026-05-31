using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naseejk.core.Entities;

namespace Naseejk.Infrastructure.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(al => al.Id);
            builder.Property(al => al.Id).ValueGeneratedNever();

            builder.Property(al => al.Timestamp)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.Property(al => al.Subject).IsRequired().HasMaxLength(200);
            builder.Property(al => al.Action).IsRequired().HasMaxLength(100);
            builder.Property(al => al.Resource).IsRequired().HasMaxLength(200);
            builder.Property(al => al.PermissionKey).IsRequired().HasMaxLength(256);
            builder.Property(al => al.Reason).HasColumnType("nvarchar(max)");
            builder.Property(al => al.TraceId).HasMaxLength(100);
            builder.Property(al => al.IpAddress).HasMaxLength(45);
            builder.Property(al => al.UserAgent).HasColumnType("nvarchar(max)");
            builder.Property(al => al.MetadataJson).HasColumnType("nvarchar(max)");

            builder.HasOne(al => al.Application)
                .WithMany(a => a.AuditLogs)
                .HasForeignKey(al => al.ApplicationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(al => al.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}