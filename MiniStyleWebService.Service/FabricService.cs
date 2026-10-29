using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniStyleWebService.Service
{
    public class FabricService : IFabricService
    {
        private readonly IFabricRepository _repo;

        public FabricService(IFabricRepository repo)
        {
            _repo = repo;
        }

        public Domain.FabricLister GetAll(Domain.FabricLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Fabric Upsert(Domain.Fabric mFabric)
        {
            return _repo.Upsert(mFabric);
        }

        public Domain.Fabric Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public List<Domain.Fabric> GetAllActiveFabric()
        {
            return _repo.GetAllActiveFabric();
        }
    }
}
