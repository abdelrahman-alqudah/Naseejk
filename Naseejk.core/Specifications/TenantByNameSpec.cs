using Naseejk.core.Entities;
namespace Naseejk.core.Specifications
{
    public class TenantByNameSpec:BaseSpecification<Tenant>
    {
        public TenantByNameSpec(string name)
            : base(t => t.Name == name)
        {
        }
    }
}
