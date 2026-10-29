using MiniStyleWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MiniStyleWebService.Repository
{
    public class ProductImageRepository : IProductImageRepository
    {
        public Domain.ProductImage Create(Domain.ProductImage mImage)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efImage = new Persistence.ProductImage();

                    efImage.ProductId = mImage.ProductId;
                    efImage.ProductColorId = mImage.ProductColorId;
                    efImage.FileName = mImage.FileName;
                    efImage.MimeType = mImage.MimeType;
                    context.ProductImages.Add(efImage);
                    context.SaveChanges();
                    mImage.Id = efImage.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mImage;
        }

        public List<Domain.ProductImage> Get(int productId)
        {
            var mImages = new List<Domain.ProductImage>();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efImages = (from pi in context.ProductImages
                                    where pi.ProductId == productId
                                    select new
                                    {
                                        Id = pi.Id,
                                        ProductId = pi.ProductId,
                                        ProductColorId = pi.ProductColorId,
                                        FileName = pi.FileName,
                                        MimeType = pi.MimeType,
                                    }).AsEnumerable();

                    mImages = JsonSerializer.Deserialize<List<Domain.ProductImage>>(JsonSerializer.Serialize(efImages.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mImages;
        }

        public List<Domain.ProductImage> Get(int productId, int productColorId)
        {
            var mImages = new List<Domain.ProductImage>();
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efImages = (from pi in context.ProductImages
                                    where pi.ProductId == productId && pi.ProductColorId == productColorId
                                    select new
                                    {
                                        Id = pi.Id,
                                        ProductId = pi.ProductId,
                                        ProductColorId = pi.ProductColorId,
                                        FileName = pi.FileName,
                                        MimeType = pi.MimeType,
                                    }).AsEnumerable();

                    if (productColorId > 0)
                    {
                        efImages = efImages.Where(x => x.ProductColorId == productColorId);
                    }

                    mImages = JsonSerializer.Deserialize<List<Domain.ProductImage>>(JsonSerializer.Serialize(efImages.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mImages;
        }

        public void Delete(int id)
        {
            try
            {
                using (var context = new Persistence.MiniStyleDB())
                {
                    var efImage = context.ProductImages.FirstOrDefault(x => x.Id == id);
                    if (efImage != null && efImage.Id > 0)
                    {
                        context.ProductImages.Remove(efImage);
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
