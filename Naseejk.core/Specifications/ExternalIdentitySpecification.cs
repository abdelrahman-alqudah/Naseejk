using Naseejk.core.Entities;

namespace Naseejk.core.Specifications
{
    public class ExternalIdentitySpecification : BaseSpecification<UserExternalIdentitie>
    {
        public ExternalIdentitySpecification(string externalId, long applicationId)
            : base(x => x.ExternalId == externalId && x.ApplicationId == applicationId)
        {
            AddInclude(x => x.User);

            AddInclude(x => x.User.UserAppRoles);
        }
    }
}