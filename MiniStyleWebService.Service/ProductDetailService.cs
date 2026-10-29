using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;

namespace MiniStyleWebService.Service
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly IProductDetailRepository _repo;

        public ProductDetailService(IProductDetailRepository repo)
        {
            _repo = repo;
        }

        public Domain.ProductDetail Upsert(Domain.ProductDetail mDetail)
        {
            return _repo.Upsert(mDetail);
        }

        public Domain.ProductDetail Get(int productId)
        {
            var mProductDetail = _repo.Get(productId);

            if (mProductDetail == null || mProductDetail.ProductDetailId == default(int))
                mProductDetail = new Domain.ProductDetail() { ProductId = productId };

            return mProductDetail;
        }   
    }
}
