using System.Collections.Generic;

namespace MiniStyleWebService.Infrastructure.IService
{
    public interface ICartService
    {
        Domain.Cart Create(Domain.Cart mCart);
        
        List<Domain.Cart> GetBy(string sessionId);
        
        void Delete(int id);
        
        Domain.CartSummary GetSummary(string sessionId);

        void UpdateQty(int id, int Qty);

        int CartCount(string sessionId);
    }
}
