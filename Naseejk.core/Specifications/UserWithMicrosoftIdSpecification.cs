using Naseejk.core.Entities;

namespace Naseejk.core.Specifications
{
    public class UserWithMicrosoftIdSpecification : BaseSpecification<User>
    {
        public UserWithMicrosoftIdSpecification(long tenantId)
            : base(u => u.TenantId == tenantId)
        {
            AddInclude(u => u.UserAppRoles);
        }
    }
}
