using Naseejk.core.Common;
using Naseejk.core.DTOs;

namespace Naseejk.core.Interfaces
{
    public interface IExternalAuthProvider
    {
        string ProviderName { get; }

        Task<Result<AuthResponseDto>> AuthenticateAsync(string idToken);
    }
}