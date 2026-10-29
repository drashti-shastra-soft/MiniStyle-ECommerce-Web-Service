using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using System.Collections.Generic;

namespace MiniStyleWebService.Service
{
    public class AgeGroupService : IAgeGroupService
    {
        private readonly IAgeGroupRepository _repo;

        public AgeGroupService(IAgeGroupRepository repo)
        {
            _repo = repo;
        }

        public Domain.AgeGroupLister GetAll(Domain.AgeGroupLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.AgeGroup Upsert(Domain.AgeGroup mAgeGroup)
        {
            return _repo.Upsert(mAgeGroup);
        }

        public Domain.AgeGroup Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public List<Domain.AgeGroup> GetAllActiveAgeGroup()
        {
            return _repo.GetAllActiveAgeGroup();
        }
    }
}
