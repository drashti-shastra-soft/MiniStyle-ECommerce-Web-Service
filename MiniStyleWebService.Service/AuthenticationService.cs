using MiniStyleWebService.Infrastructure.CustomException;
using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using MiniStyleWebService.Infrastructure.Utils;
using System.Configuration;

namespace MiniStyleWebService.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _repo;
        private readonly IJWTokenService _tokenService;

        public AuthenticationService(IUserRepository repo, IJWTokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }

        public Domain.User Authenticate(Domain.Credential mCredential)
        {
            var mUser = new Domain.User();

            if (mCredential == null)
                throw new APIRequestFailedException("Object must have a value!");

            if (mCredential.EmailAddress.IsNullOrEmpty())
                throw new APIRequestFailedException("Email Address is required!");

            if (mCredential.Password.IsNullOrEmpty())
                throw new APIRequestFailedException("Password is required!");

            mUser = _repo.GetBy(mCredential.EmailAddress);
            if (mUser == null)
                throw new APIRequestFailedException("Authentication failed: user identity could not be verified!");

            if (mUser.Password != PasswordEncryptor.Encrypt(mCredential.Password, ConfigurationManager.AppSettings.Get("Salt")))
                throw new APIRequestFailedException("Authentication failed: user identity could not be verified!");

            mUser.Token = _tokenService.GenerateJwtToken(mUser);

            return mUser;
        }
    }
}
