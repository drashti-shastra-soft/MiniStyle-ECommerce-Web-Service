using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Infrastructure.IService
{
    public interface IOrderLineService
    {
        List<Domain.OrderLine> Get(int id);
    }
}
