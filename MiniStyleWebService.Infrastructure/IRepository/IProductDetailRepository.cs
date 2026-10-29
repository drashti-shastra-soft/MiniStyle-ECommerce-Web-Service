namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface IProductDetailRepository
    {
        Domain.ProductDetail Upsert(Domain.ProductDetail mDetail);

        Domain.ProductDetail Get(int productId);
    }
}
