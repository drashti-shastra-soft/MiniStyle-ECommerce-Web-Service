using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using MiniStyleWebService.Domain;
using System.Collections.Generic;

namespace MiniStyleWebService.Service
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _repo;

        public SubCategoryService(ISubCategoryRepository repo)
        {
            _repo = repo;
        }

        public Domain.SubCategoryLister GetAll(Domain.SubCategoryLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.SubCategory Upsert(Domain.SubCategory mSubCategory)
        {
            return _repo.Upsert(mSubCategory);
        }

        public Domain.SubCategory Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }


        public List<SubCategory> GetAllActiveSubCategories()
        {
            return _repo.GetAllActiveSubCategories();
        }

        public List<SubCategory> GetAllActiveSubCategoriesBy(int categoryId)
        {
            return _repo.GetAllActiveSubCategoriesBy(categoryId);
        }
    }
}
