using System.Collections.Generic;
using System;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface ISubCategoryRepository
    {
        Domain.SubCategoryLister GetAll(Domain.SubCategoryLister mLister);

        Domain.SubCategory Upsert(Domain.SubCategory mSubCategory);

        Domain.SubCategory Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.SubCategory> GetAllActiveSubCategories();

        List<Domain.SubCategory> GetAllActiveSubCategoriesBy(int categoryId);
    }
}
