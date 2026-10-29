using System;
using System.Collections.Generic;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface IProductImageRepository
    {
        Domain.ProductImage Create(Domain.ProductImage mImage);

        List<Domain.ProductImage> Get(int productId);

        List<Domain.ProductImage> Get(int productId, int productColorId);

        void Delete(int id);
       
    }
}
