using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface IFabricRepository
    {
        Domain.FabricLister GetAll(Domain.FabricLister mLister);

        Domain.Fabric Upsert(Domain.Fabric mFabric);

        Domain.Fabric Get(int id);

        List<Domain.Fabric> GetAllActiveFabric();

        void Delete(int id, int loginUserId);
    }
}
