using MiniStyleWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MiniStyleWebService.Repository
{
    public class OrderLineRepository : IOrderLineRepository
    {
        public List<Domain.OrderLine> Get(int id)
        {
            var mOrderLines = new List<Domain.OrderLine>();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efOrderLines = (from ol in context.OrderLines
                                        join p in context.Products on ol.ProductId equals p.Id
                                        where ol.OrderId == id
                                        select new
                                        {
                                            Id = ol.Id,
                                            OrderId = ol.OrderId,
                                            ProductId = ol.ProductId,
                                            Product = p.Name,
                                            ProductCode = ol.ProductCode,
                                            Size = ol.Size,
                                            Color = ol.Color,
                                            Qty = ol.Qty,
                                            UnitPrice = Math.Round(ol.UnitPrice, 2),
                                            DiscountPercentage = ol.DiscountPercentage,
                                            TotalPrice = ol.TotalPrice.HasValue ? Math.Round(ol.TotalPrice.Value, 2) : default(decimal),
                                        }).AsEnumerable();

                    mOrderLines = JsonSerializer.Deserialize<List<Domain.OrderLine>>(JsonSerializer.Serialize(efOrderLines.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mOrderLines;
        }

        public void Create(Domain.OrderLine mLine)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {

                    var efLine = new Persistence.OrderLine();
                    efLine.OrderId = mLine.OrderId;
                    efLine.ProductId = mLine.ProductId;
                    efLine.ProductCode = mLine.ProductCode;
                    efLine.Size = mLine.Size;
                    efLine.Color = mLine.Color;
                    efLine.Qty = mLine.Qty;
                    efLine.UnitPrice = mLine.UnitPrice;
                    efLine.DiscountPercentage = mLine.DiscountPercentage;
                    efLine.TotalPrice = mLine.TotalPrice;
                    context.OrderLines.Add(efLine);
                    context.SaveChanges();
                    mLine.Id = efLine.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}