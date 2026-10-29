using MiniStyleWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MiniStyleWebService.Repository
{
    public class ProductColorRepository : IProductColorRepository
    {
        public void Create(List<Domain.ProductColor> mColors)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    foreach (var item in mColors)
                    {
                        var efColor = new Persistence.ProductColor();
                        efColor.ProductId = item.ProductId;
                        efColor.ColorId = item.ColorId;
                        context.ProductColors.Add(efColor);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Domain.ProductColor> Get(int productId)
        {
            var mColors = new List<Domain.ProductColor>();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efColors = (from pc in context.ProductColors
                                    join c in context.Colors on pc.ColorId equals c.Id
                                    where pc.ProductId == productId
                                    select new
                                    {
                                        Id = pc.Id,
                                        ProductId = pc.ProductId,
                                        ColorId = pc.ColorId,
                                        Color = c.Name,
                                        HashCode = c.HashCode
                                    }).AsEnumerable();

                    mColors = JsonSerializer.Deserialize<List<Domain.ProductColor>>(JsonSerializer.Serialize(efColors.ToList()));
                }

            }
            catch (Exception)
            {
                throw;
            }
            return mColors;
        }

        public void Delete(int productId)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efColors = context.ProductColors.Where(x => x.ProductId == productId).ToList();
                    if (efColors != null && efColors.Count > 0)
                    {
                        foreach (var item in efColors)
                        {
                            context.ProductColors.Remove(item);
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
