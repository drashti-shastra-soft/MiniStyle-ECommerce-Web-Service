using System;
using System.Collections.Generic;

namespace MiniStyleWebService.Infrastructure.IService
{
    public interface IAgeGroupService
    {
        Domain.AgeGroupLister GetAll(Domain.AgeGroupLister mLister);

        Domain.AgeGroup Upsert(Domain.AgeGroup mEmployee);

        Domain.AgeGroup Get(int id);

        List<Domain.AgeGroup> GetAllActiveAgeGroup();

        void Delete(int id, int loginUserId);

    }
}
