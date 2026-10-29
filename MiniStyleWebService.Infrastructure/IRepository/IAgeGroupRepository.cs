using System;
using System.Collections.Generic;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface IAgeGroupRepository
    {
        Domain.AgeGroupLister GetAll(Domain.AgeGroupLister mLister);

        Domain.AgeGroup Upsert(Domain.AgeGroup mAgeGroup);

        Domain.AgeGroup Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.AgeGroup> GetAllActiveAgeGroup();
    }
}
