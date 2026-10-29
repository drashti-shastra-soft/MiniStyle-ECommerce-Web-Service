using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using System.Collections.Generic;
using System.Linq;

namespace MiniStyleWebService.Service
{
    public class ProductAgeGroupService : IProductAgeGroupService
    {
        private readonly IProductAgeGroupRepository _repo;

        public ProductAgeGroupService(IProductAgeGroupRepository repo)
        {
            _repo = repo;
        }

        public void Upsert(List<Domain.ProductAgeGroup> mAgeGroup)
        {
            var productId = mAgeGroup.Select(x => x.ProductId).FirstOrDefault();
            if (productId > default(int))
            {
                _repo.Delete(productId);
            }            
            _repo.Create(mAgeGroup);
        }

        public List<Domain.ProductAgeGroup> Get(int productId)
        {
            return _repo.Get(productId);
        }
    }
}
