using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using MiniStyleWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace MiniStyleWebService.Service
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _repo;

        public ProductImageService(IProductImageRepository repo)
        {
            _repo = repo;
        }

        public Domain.ProductImage Create(Domain.ProductImage mImage)
        {
            if (mImage == null)
                throw new Infrastructure.CustomException.APIRequestFailedException("Object must have a value!");

            if (mImage.Base64String.IsNullOrEmpty())
                throw new Infrastructure.CustomException.APIRequestFailedException("Image is required!");

            string relativeFolder = "~/Images/";
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(mImage.FileName);
            mImage.FileName = uniqueFileName;
            StaticMethods.SaveBase64Image(mImage.Base64String, relativeFolder, uniqueFileName);

            return _repo.Create(mImage);
        }

        public List<Domain.ProductImage> Get(int productId)
        {
            var mImages = _repo.Get(productId);

            if (mImages != null && mImages.Count > 0)
            {
                foreach (var item in mImages)
                {
                    item.Base64String = StaticMethods.GetImageBase64(HostingEnvironment.MapPath($"~/Images/{item.FileName}"));
                }
            }

            return mImages;
        }

        public List<Domain.ProductImage> Get(int productId, int productColorId)
        {
            var mImages = _repo.Get(productId, productColorId);
            if (mImages != null && mImages.Count > 0)
            {
                foreach (var item in mImages)
                {
                    item.Base64String = StaticMethods.GetImageBase64(HostingEnvironment.MapPath($"~/Images/{item.FileName}"));
                }
            }
            return mImages;
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }
    }
}
