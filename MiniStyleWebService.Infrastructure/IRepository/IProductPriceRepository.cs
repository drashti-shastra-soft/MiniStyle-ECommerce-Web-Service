using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface IProductPriceRepository
    {
        Domain.ProductPrice Upsert(Domain.ProductPrice mProductPrice);
        
        Domain.ProductPrice Get(int productId);
    }
}
