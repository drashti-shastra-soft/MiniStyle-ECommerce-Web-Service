using System.Collections.Generic;

namespace MiniStyleWebService.Infrastructure.IService
{
    public interface IProductColorService
    {
        void Upsert(List<Domain.ProductColor> mColors);

        List<Domain.ProductColor> Get(int productId);
    }
}
