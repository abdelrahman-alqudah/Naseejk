using Naseejk.core.Entities;
namespace Naseejk.core.Specifications
{
    public class ApplicationByClientIdSpec : BaseSpecification<Application>
    {
        public ApplicationByClientIdSpec(string clientId)
            : base(a => a.ClientIdentifier == clientId)
        {
            AddInclude(a => a.Roles);
        }
    }
}
