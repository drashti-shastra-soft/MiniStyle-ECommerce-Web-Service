using System.Collections.Generic;

namespace MiniStyleWebService.Infrastructure.IService
{
    public interface IProductAgeGroupService
    {
        void Upsert(List<Domain.ProductAgeGroup> mAgeGroup);

        List<Domain.ProductAgeGroup> Get(int productId);
    }
}
