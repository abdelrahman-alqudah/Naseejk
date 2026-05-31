using Naseejk.core.Entities;

namespace Naseejk.core.Specifications
{
    public class ApplicationWithMicrosoftIdSpecification:BaseSpecification<Application>
    {
        public ApplicationWithMicrosoftIdSpecification(string  clientId)
            : base(a => a.ClientIdentifier == clientId && a.IsActive == true)
        {
        }
    }
}
