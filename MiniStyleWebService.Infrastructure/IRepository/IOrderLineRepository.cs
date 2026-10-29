using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface IOrderLineRepository
    {
        List<Domain.OrderLine> Get(int id);

        void Create(Domain.OrderLine mLine);
    }
}
