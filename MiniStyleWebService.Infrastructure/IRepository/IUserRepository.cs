using System;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface IUserRepository
    {
        Domain.UserLister GetAll(Domain.UserLister mLister);

        Domain.User Upsert(Domain.User mUser);

        Domain.User Get(int id);

        void Delete(int id, int loginUserId);

        Domain.User GetBy(string emailAddress);

        String GetPasswordBy(int id);

        void SetPasswordBy(int id, string password);

        bool CheckEmailAddressExist(Domain.User mUser);
    }
}
