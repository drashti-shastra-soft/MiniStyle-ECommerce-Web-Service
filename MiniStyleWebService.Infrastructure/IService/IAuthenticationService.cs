using System;

namespace MiniStyleWebService.Infrastructure.IService
{
    public interface IAuthenticationService
    {
        Domain.User Authenticate(Domain.Credential mCredential);
    }
}
