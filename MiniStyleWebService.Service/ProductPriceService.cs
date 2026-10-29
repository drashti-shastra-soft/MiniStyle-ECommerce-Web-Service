using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;

namespace MiniStyleWebService.Service
{
    public class ProductPriceService : IProductPriceService
    {
        private readonly IProductPriceRepository _repo;

        public ProductPriceService(IProductPriceRepository repo)
        {
            _repo = repo;
        }

        public Domain.ProductPrice Upsert(Domain.ProductPrice mProductPrice)
        {
            return _repo.Upsert(mProductPrice);
        }

        public Domain.ProductPrice Get(int productId)
        {
            var mProductPrice = _repo.Get(productId);

            if (mProductPrice == null || mProductPrice.Id == default(int))
                mProductPrice = new Domain.ProductPrice() { ProductId = productId };

            return mProductPrice;
        }
    }
}
