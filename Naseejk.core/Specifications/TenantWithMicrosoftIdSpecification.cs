using Naseejk.core.Entities;

namespace Naseejk.core.Specifications
{
    public class TenantWithMicrosoftIdSpecification:BaseSpecification<Tenant>
    {
        public TenantWithMicrosoftIdSpecification(string microsoftTenantId)
            :base(t => t.MicrosoftTenantId == microsoftTenantId)
        {
        }

    }
}
