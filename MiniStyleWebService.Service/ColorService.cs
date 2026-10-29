using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using System.Collections.Generic;

namespace MiniStyleWebService.Service
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _repo;

        public ColorService(IColorRepository repo)
        {
            _repo = repo;
        }

        public Domain.ColorLister GetAll(Domain.ColorLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Color Upsert(Domain.Color mColor)
        {
            return _repo.Upsert(mColor);
        }

        public Domain.Color Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public List<Domain.Color> GetAllActiveColor()
        {
            return _repo.GetAllActiveColor();
        }
    }
}
