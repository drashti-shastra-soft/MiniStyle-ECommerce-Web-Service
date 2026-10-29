using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Service
{
    public class OrderLineService : IOrderLineService
    {
        private readonly IOrderLineRepository _repo;

        public OrderLineService(IOrderLineRepository repo)
        {
            _repo = repo;
        }

        public List<Domain.OrderLine> Get(int id)
        {
            return _repo.Get(id);
        }

    }
}
