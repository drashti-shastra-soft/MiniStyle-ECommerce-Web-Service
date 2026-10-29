using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Service
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _repo;
        private readonly IOrderLineRepository _orderLineRepo;
        private readonly ICartRepository _cartRepo;

        public OrderService(IOrderRepository repo, IOrderLineRepository orderLineRepo, ICartRepository cartRepo)
        {
            _repo = repo;
            _orderLineRepo = orderLineRepo;
            _cartRepo = cartRepo;
        }

        public Domain.OrderLister GetAll(Domain.OrderLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Order Upsert(Domain.Order mOrder)
        {
            var mCreatedOrder = _repo.Upsert(mOrder);
            if (mCreatedOrder != null && mCreatedOrder.Id > 0)
            {
                var mCartItems = _cartRepo.GetBy(mCreatedOrder.SessionId);
                if (mCartItems != null && mCartItems.Count > 0)
                {
                    foreach (var item in mCartItems)
                    {
                        var mOrderItem = ConvertCartItemsToOrderLines(mCreatedOrder.Id, item);
                        _orderLineRepo.Create(mOrderItem);
                        _cartRepo.Delete(item.Id);
                    }
                }
            }
            return mCreatedOrder;
        }

        public Domain.Order Get(int id)
        {
            return _repo.Get(id);
        }

        public void UpdateStatus(Domain.Order mOrder)
        {
            _repo.UpdateStatus(mOrder);
        }

        private Domain.OrderLine ConvertCartItemsToOrderLines(int orderId, Domain.Cart mCartItems)
        {
            var mOrderLine = new Domain.OrderLine();
            mOrderLine.OrderId = orderId;
            mOrderLine.ProductId = mCartItems.ProductId;
            mOrderLine.ProductCode = mCartItems.ProductCode;
            mOrderLine.Size = mCartItems.Size;
            mOrderLine.Color = mCartItems.Color;
            mOrderLine.Qty = mCartItems.Qty;
            mOrderLine.UnitPrice = mCartItems.UnitPrice;
            mOrderLine.DiscountPercentage = mCartItems.DiscountPercentage;
            mOrderLine.TotalPrice = mCartItems.TotalPrice;

            return mOrderLine;
        }
    }
}
