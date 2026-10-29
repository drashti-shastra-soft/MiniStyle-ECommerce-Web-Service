namespace MiniStyleWebService.Infrastructure.IService
{
    public interface IProductService
    {
        Domain.ProductLister GetAll(Domain.ProductLister mLister);

        Domain.Product Upsert(Domain.Product mProduct);

        Domain.Product Get(int id);

        void Delete(int id, int loginUserId);

        Domain.ProductLister GetAllActiveProducts(Domain.ProductLister mLister);

        Domain.ProductViewModel GetProductDetails(int id);
    }
}
