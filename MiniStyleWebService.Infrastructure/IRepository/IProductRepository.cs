using System;
using System.Collections.Generic;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface IProductRepository
    {
        Domain.ProductLister GetAll(Domain.ProductLister mLister);

        Domain.Product Upsert(Domain.Product mProduct);

        Domain.Product Get(int id);

        void Delete(int id, int loginUserId);

        Domain.ProductLister GetAllActiveProducts(Domain.ProductLister mLister);
    }
}
