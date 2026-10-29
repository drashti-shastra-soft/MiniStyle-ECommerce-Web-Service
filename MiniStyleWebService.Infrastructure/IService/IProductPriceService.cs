using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Infrastructure.IService
{
    public interface IProductPriceService
    {
        Domain.ProductPrice Upsert(Domain.ProductPrice mProductPrice);
        
        Domain.ProductPrice Get(int productId);
    }
}
