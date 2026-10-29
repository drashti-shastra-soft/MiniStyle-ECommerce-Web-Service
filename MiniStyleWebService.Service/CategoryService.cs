using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using MiniStyleWebService.Domain;
using System.Collections.Generic;

namespace MiniStyleWebService.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public List<Category> GetAll()
        {
            return _repo.GetAll();
        }
    }
}
