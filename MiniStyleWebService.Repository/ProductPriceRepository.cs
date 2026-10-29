using MiniStyleWebService.Infrastructure.IRepository;
using System;
using System.Linq;
using System.Text.Json;

namespace MiniStyleWebService.Repository
{
    public class ProductPriceRepository : IProductPriceRepository
    {
        public Domain.ProductPrice Upsert(Domain.ProductPrice mProductPrice)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efProductPrice = context.ProductPrices.Where(x => x.Id == mProductPrice.Id).FirstOrDefault();
                    if (efProductPrice == null || efProductPrice.Id <= default(int))
                    {
                        efProductPrice = new Persistence.ProductPrice();
                        context.ProductPrices.Add(efProductPrice);
                    }
                    efProductPrice.ProductId = mProductPrice.ProductId;
                    efProductPrice.PurchasePrice = mProductPrice.PurchasePrice;
                    efProductPrice.SellPrice = mProductPrice.SellPrice;
                    efProductPrice.DiscountPercentage = mProductPrice.DiscountPercentage;

                    context.SaveChanges();

                    mProductPrice.Id = efProductPrice.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mProductPrice;
        }

        public Domain.ProductPrice Get(int productId)
        {
            var mProductPrice = new Domain.ProductPrice();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efProductPrice = (from p in context.ProductPrices
                                          where p.ProductId == productId
                                          select new
                                          {
                                              Id = p.Id,
                                              ProductId = p.ProductId,
                                              PurchasePrice = p.PurchasePrice,
                                              SellPrice = p.SellPrice,
                                              DiscountPercentage = p.DiscountPercentage,
                                          }).AsEnumerable();

                    mProductPrice = JsonSerializer.Deserialize<Domain.ProductPrice>(JsonSerializer.Serialize(efProductPrice.FirstOrDefault()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mProductPrice;
        }
    }
}
