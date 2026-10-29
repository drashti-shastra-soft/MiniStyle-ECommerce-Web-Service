using System.Collections.Generic;
using System;

namespace MiniStyleWebService.Infrastructure.IService
{
    public interface IMicroCategoryService
    {
        Domain.MicroCategoryLister GetAll(Domain.MicroCategoryLister mLister);

        Domain.MicroCategory Upsert(Domain.MicroCategory mMicroCategory);

        Domain.MicroCategory Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.MicroCategory> GetAllActiveMicroCategories();

    }
}
