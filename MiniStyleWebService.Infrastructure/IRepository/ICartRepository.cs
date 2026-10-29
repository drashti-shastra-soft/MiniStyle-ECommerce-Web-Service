using System.Collections.Generic;

namespace MiniStyleWebService.Infrastructure.IRepository
{
    public interface ICartRepository
    {
        Domain.Cart Create(Domain.Cart mCart);
        
        List<Domain.Cart> GetBy(string sessionId);
        
        void Delete(int id);

        void UpdateQty(int id, int Qty);

        int CartCount(string sessionId);
    }
}
