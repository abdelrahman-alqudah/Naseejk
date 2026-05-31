using Naseejk.core.Entities;

namespace Naseejk.core.Specifications
{
    public class UserWithEmailSpecification : BaseSpecification<User>
    {
        public UserWithEmailSpecification(string email,long tenantId)
            : base(u => u.Email == email&& u.TenantId == tenantId)
        {
            AddInclude(u => u.UserAppRoles);
        }
    }
}
