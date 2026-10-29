using System.Collections.Generic;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface IProductAgeGroupRepository
    {
        void Create(List<Domain.ProductAgeGroup> mAgeGroup);

        List<Domain.ProductAgeGroup> Get(int productId);

        void Delete(int productId);
    }
}
