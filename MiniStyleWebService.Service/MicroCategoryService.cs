using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using System.Collections.Generic;

namespace MiniStyleWebService.Service
{
    public class MicroCategoryService : IMicroCategoryService
    {
        private readonly IMicroCategoryRepository _repo;

        public MicroCategoryService(IMicroCategoryRepository repo)
        {
            _repo = repo;
        }

        public Domain.MicroCategoryLister GetAll(Domain.MicroCategoryLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.MicroCategory Upsert(Domain.MicroCategory mMicroCategory)
        {
            return _repo.Upsert(mMicroCategory);
        }

        public Domain.MicroCategory Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public List<Domain.MicroCategory> GetAllActiveMicroCategories()
        {
            return _repo.GetAllActiveMicroCategories();
        }
    }
}
