using System;
using System.Collections.Generic;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface IColorRepository
    {
        Domain.ColorLister GetAll(Domain.ColorLister mLister);

        Domain.Color Upsert(Domain.Color mColor);

        Domain.Color Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.Color> GetAllActiveColor();
    }
}
