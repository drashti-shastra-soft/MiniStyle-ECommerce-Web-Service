using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using System.Collections.Generic;

namespace MiniStyleWebService.Service
{
    public class ProductSizeChartService : IProductSizeChartService
    {
        private readonly IProductSizeChartRepository _repo;

        public ProductSizeChartService(IProductSizeChartRepository repo)
        {
            _repo = repo;
        }

        public Domain.ProductSizeChart Upsert(Domain.ProductSizeChart mProductSizeChart)
        {
            return _repo.Upsert(mProductSizeChart);
        }

        public Domain.ProductSizeChart Get(int id)
        {
            return _repo.Get(id);
        }

        public List<Domain.ProductSizeChart> GetAllActive(int productId, string unitType)
        {
            return _repo.GetAllActive(productId, unitType);
        }

        public void Delete(int id, int userId, int loginUserId)
        {
            _repo.Delete(id, userId, loginUserId);
        }
    }
}
