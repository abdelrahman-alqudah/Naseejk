using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Naseejk.core.Entities;

namespace Naseejk.Infrastructure.Configurations
{
    public class TenantConfiguration:IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenants");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(t => t.MicrosoftTenantId)
                .HasMaxLength(256);

            builder.HasIndex(t => t.MicrosoftTenantId)
                .IsUnique()
                .HasFilter("[MicrosoftTenantId] IS NOT NULL"); 

            builder.Property(t => t.IsActive)
                .IsRequired()
                .HasDefaultValue(true); 

            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasMany(t => t.Users)
                .WithOne(u => u.Tenant)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.NoAction); 
        }
    }
}
