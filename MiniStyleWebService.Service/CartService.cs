using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using MiniStyleWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;

namespace MiniStyleWebService.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repo;

        public CartService(ICartRepository repo)
        {
            _repo = repo;
        }

        public Domain.Cart Create(Domain.Cart mCart)
        {
            mCart.DiscountedPrice = Math.Round(mCart.UnitPrice - (mCart.UnitPrice * mCart.DiscountPercentage / 100), 2);
            mCart.TotalPrice = (mCart.Qty * mCart.DiscountedPrice);
            return _repo.Create(mCart);
        }

        public List<Domain.Cart> GetBy(string sessionId)
        {
            var mCartItems = _repo.GetBy(sessionId);

            if (mCartItems != null && mCartItems.Count > 0)
            {
                foreach (var item in mCartItems)
                {
                    item.DiscountedPrice = Math.Round(item.UnitPrice - (item.UnitPrice * item.DiscountPercentage / 100), 2);

                    //Bind Base64String
                    if (item.FileName.IsNullOrEmpty())
                        item.ImageBase64String = StaticMethods.GetImageBase64(HostingEnvironment.MapPath("~/Images/No-Image-Found.jpg"));
                    else
                        item.ImageBase64String = StaticMethods.GetImageBase64(HostingEnvironment.MapPath($"~/Images/{item.FileName}"));
                }
            }

            return mCartItems;
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }

        public Domain.CartSummary GetSummary(string sessionId)
        {
            var mSummary = new Domain.CartSummary();

            var mCartItems = _repo.GetBy(sessionId);
            if (mCartItems != null && mCartItems.Count > 0)
            {
                foreach (var item in mCartItems)
                {
                    mSummary.TotalPrice += item.UnitPrice * item.Qty;
                    mSummary.Discounted += (Math.Round((mSummary.TotalPrice * item.DiscountPercentage / 100), 2));
                }

                mSummary.ShippingFee = 50;
                mSummary.Total = Math.Ceiling(mSummary.ShippingFee + mCartItems.Sum(x => x.TotalPrice));
            }
            return mSummary;
        }

        public void UpdateQty(int id, int Qty)
        {
            _repo.UpdateQty(id, Qty);
        }

        public int CartCount(string sessionId)
        {
            return _repo.CartCount(sessionId);
        }

    }
}
