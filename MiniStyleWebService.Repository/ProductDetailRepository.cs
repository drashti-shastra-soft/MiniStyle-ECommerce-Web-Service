using MiniStyleWebService.Infrastructure.IRepository;
using System;
using System.Linq;
using System.Text.Json;

namespace MiniStyleWebService.Repository
{
    public class ProductDetailRepository : IProductDetailRepository
    {
        public Domain.ProductDetail Upsert(Domain.ProductDetail mDetail)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efDetail = context.ProductDetails.Where(x => x.Id == mDetail.ProductDetailId).FirstOrDefault();
                    if (efDetail == null || efDetail.Id <= default(int))
                    {
                        efDetail = new Persistence.ProductDetail();
                        context.ProductDetails.Add(efDetail);
                    }
                    efDetail.ProductId = mDetail.ProductId;
                    efDetail.Description = mDetail.Description;
                    efDetail.Specification = mDetail.Specification;
                    efDetail.FabricId = mDetail.FabricId;

                    context.SaveChanges();

                    mDetail.ProductDetailId = efDetail.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mDetail;
        }

        public Domain.ProductDetail Get(int productId)
        {
            var mDetail = new Domain.ProductDetail();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efDetail = (from pd in context.ProductDetails
                                    join f in context.Fabrics on pd.FabricId equals f.Id into joinPDF
                                    from f in joinPDF.DefaultIfEmpty()
                                    where pd.ProductId == productId && f.IsActive
                                    select new
                                    {
                                        ProductDetailId = pd.Id,
                                        ProductId = pd.ProductId,
                                        Description = pd.Description,
                                        Specification = pd.Specification,
                                        FabricId = pd.FabricId,
                                        Fabric = f.Name
                                    }).AsEnumerable();

                    mDetail = JsonSerializer.Deserialize<Domain.ProductDetail>(JsonSerializer.Serialize(efDetail.FirstOrDefault()));
                }

            }
            catch (Exception)
            {
                throw;
            }

            return mDetail;
        }
    }
}
