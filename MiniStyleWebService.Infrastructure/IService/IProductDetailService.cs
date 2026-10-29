using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Infrastructure.IService
{
    public interface IProductDetailService
    {
        Domain.ProductDetail Upsert(Domain.ProductDetail mDetail);

        Domain.ProductDetail Get(int productId);
    }
}
