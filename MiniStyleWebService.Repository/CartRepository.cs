using MiniStyleWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

namespace MiniStyleWebService.Repository
{
    public class CartRepository : ICartRepository
    {
        public Domain.Cart Create(Domain.Cart mCart)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efCart = new Persistence.Cart();
                    efCart.SessionId = mCart.SessionId;
                    efCart.ProductId = mCart.ProductId;
                    efCart.ProductCode = mCart.ProductCode;
                    efCart.Size = mCart.Size;
                    efCart.Color = mCart.Color;
                    efCart.Qty = mCart.Qty;
                    efCart.UnitPrice = mCart.UnitPrice;
                    efCart.DiscountPercentage = mCart.DiscountPercentage;
                    context.Carts.Add(efCart);
                    context.SaveChanges();
                    mCart.Id = efCart.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mCart;
        }

        public List<Domain.Cart> GetBy(string sessionId)
        {
            var mCartItems = new List<Domain.Cart>();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efCartItems = (from c in context.Carts
                                       join p in context.Products on c.ProductId equals p.Id
                                       let firstImage = context.ProductImages.Where(pi => pi.ProductId == p.Id).OrderBy(pi => pi.Id).FirstOrDefault()
                                       where c.SessionId == sessionId
                                       select new
                                       {
                                           Id = c.Id,
                                           sessionId = c.SessionId,
                                           ProductId = c.ProductId,
                                           ProductName = p.Name,
                                           ProductCode = c.ProductCode,
                                           Code = c.ProductCode,
                                           Color = c.Color,
                                           Size = c.Size,
                                           Qty = c.Qty,
                                           UnitPrice = c.UnitPrice,
                                           DiscountPercentage = c.DiscountPercentage,
                                           TotalPrice = c.TotalPrice,
                                           FileName = firstImage != null ? firstImage.FileName : null
                                       }).AsEnumerable();

                    mCartItems = JsonSerializer.Deserialize<List<Domain.Cart>>(JsonSerializer.Serialize(efCartItems.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mCartItems;
        }

        public void Delete(int id)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efCart = context.Carts.Where(x => x.Id == id).FirstOrDefault();
                    if (efCart != null && efCart.Id > default(int))
                    {
                        context.Carts.Remove(efCart);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void UpdateQty(int id, int Qty)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efCart = context.Carts.Where(x => x.Id == id).FirstOrDefault();
                    if (efCart != null && efCart.Id > default(int))
                    {
                        efCart.Qty = Qty;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int CartCount(string sessionId)
        {
            int cartCount = 0;
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    cartCount = context.Carts.Count(c => c.SessionId == sessionId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return cartCount;
        }


    }
}
