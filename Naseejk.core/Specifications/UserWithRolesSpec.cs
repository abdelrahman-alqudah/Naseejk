using Naseejk.core.Entities;

namespace Naseejk.core.Specifications
{
    public class UserWithRolesSpec : BaseSpecification<User>
    {

        public UserWithRolesSpec(string email)
            : base(u => u.Email == email) 
        {
            AddInclude(u => u.UserAppRoles);        
        }
    }
}