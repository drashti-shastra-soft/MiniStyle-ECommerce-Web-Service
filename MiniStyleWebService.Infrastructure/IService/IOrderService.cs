using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Infrastructure.IService
{
    public interface IOrderService
    {
        Domain.OrderLister GetAll(Domain.OrderLister mLister);
        Domain.Order Upsert(Domain.Order mOrder);

        Domain.Order Get(int id);
        void UpdateStatus(Domain.Order mOrder);
    }
}
