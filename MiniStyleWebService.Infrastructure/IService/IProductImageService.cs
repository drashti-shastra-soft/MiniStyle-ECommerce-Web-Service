using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniStyleWebService.Infrastructure.IService
{
    public interface IProductImageService
    {
        Domain.ProductImage Create(Domain.ProductImage mImage);

        List<Domain.ProductImage> Get(int productId);

        List<Domain.ProductImage> Get(int productId, int productColorId);

        void Delete(int id);

    }
}
