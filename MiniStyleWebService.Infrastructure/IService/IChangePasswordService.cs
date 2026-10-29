using System;

namespace MiniStyleWebService.Infrastructure.IService
{
    public interface IChangePasswordService
    {
        void Upsert(Domain.ChangePassword mChangePassword);
    }
}
