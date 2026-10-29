using System;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface IChangePasswordRepository
    {
        Domain.ChangePassword UpdateUserPassword(Domain.ChangePassword mChangePassword);

        Domain.ChangePassword Get(int id);
    }
}
