using System.Collections.Generic;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface IProductColorRepository
    {
        void Create(List<Domain.ProductColor> mColors);

        List<Domain.ProductColor> Get(int productId);

        void Delete(int productId);
    }
}
