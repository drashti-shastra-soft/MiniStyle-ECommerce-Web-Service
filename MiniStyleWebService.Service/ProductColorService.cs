using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using System.Collections.Generic;
using System.Linq;

namespace MiniStyleWebService.Service
{
    public class ProductColorService : IProductColorService
    {
        private readonly IProductColorRepository _repo;

        public ProductColorService(IProductColorRepository repo)
        {
            _repo = repo;
        }

        public void Upsert(List<Domain.ProductColor> mColors)
        {
            var productId = mColors.Select(x => x.ProductId).FirstOrDefault();
            if (productId > default(int))
            {
                _repo.Delete(productId);
            }
            _repo.Create(mColors);
        }

        public List<Domain.ProductColor> Get(int productId)
        {
            return _repo.Get(productId);
        }
    }
}
