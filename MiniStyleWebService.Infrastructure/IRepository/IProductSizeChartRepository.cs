using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface IProductSizeChartRepository
    {
        Domain.ProductSizeChart Upsert(Domain.ProductSizeChart mProductSizeChart);

        Domain.ProductSizeChart Get(int id);

        List<Domain.ProductSizeChart> GetAllActive(int productId, string unitType);

        void Delete(int id, int userId, int loginUserId);
    }
}
