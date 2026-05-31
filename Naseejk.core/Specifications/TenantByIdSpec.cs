using Naseejk.core.Entities;

namespace Naseejk.core.Specifications
{
    public class TenantByIdSpec:BaseSpecification<Tenant>
    {
        public TenantByIdSpec(long id)
            : base(t => t.Id == id)
        {
        }
    }
}
