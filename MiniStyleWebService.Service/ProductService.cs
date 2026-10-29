using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using MiniStyleWebService.Infrastructure.Utils;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Hosting;

namespace MiniStyleWebService.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly IProductDetailRepository _detailRepo;
        private readonly IProductAgeGroupRepository _ageGroupRepo;
        private readonly IProductImageRepository _productImageRepo;
        private readonly IProductPriceRepository _productPrice;
        private readonly IProductColorRepository _colorRepo;
        private readonly string _domainBaseUrl;

        public ProductService(IProductRepository repo, IProductDetailRepository detailRepo, IProductImageRepository productImageRepo, IProductAgeGroupRepository ageGroupRepo, IProductPriceRepository productPrice, IProductColorRepository colorRepo)
        {
            _repo = repo;
            _detailRepo = detailRepo;
            _productImageRepo = productImageRepo;
            _domainBaseUrl = ConfigurationManager.AppSettings["DomainBaseUrl"];
            _ageGroupRepo = ageGroupRepo;
            _productPrice = productPrice;
            _colorRepo = colorRepo;
        }

        public Domain.ProductLister GetAll(Domain.ProductLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Product Upsert(Domain.Product mProduct)
        {
            return _repo.Upsert(mProduct);
        }

        public Domain.Product Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public Domain.ProductLister GetAllActiveProducts(Domain.ProductLister mLister)
        {
            mLister = _repo.GetAllActiveProducts(mLister);
            if (mLister == null || mLister.List == null || mLister.List.Count <= default(int))
            {
                return null;
            }

            foreach (var mProduct in mLister.List)
            {
                mProduct.DiscountedPrice = Convert.ToDecimal(Math.Round(Convert.ToDouble(mProduct.SellPrice - (mProduct.SellPrice * mProduct.DiscountPercentage / 100)), 2));

                if (mProduct.ImageFileName.IsNullOrEmpty())
                    mProduct.ImageBase64String = StaticMethods.GetImageBase64(HostingEnvironment.MapPath("~/Images/No-Image-Found.jpg"));
                else
                    mProduct.ImageBase64String = StaticMethods.GetImageBase64(HostingEnvironment.MapPath($"~/Images/{mProduct.ImageFileName}"));
            }

            return mLister;
        }

        public Domain.ProductViewModel GetProductDetails(int id)
        {
            var mVM = new Domain.ProductViewModel();

            mVM.Product = _repo.Get(id);
            mVM.ProductDetail = _detailRepo.Get(id);
            mVM.ProductPrice = _productPrice.Get(id);
            mVM.AgeGroup = _ageGroupRepo.Get(id);
            mVM.Color = _colorRepo.Get(id);

            if (mVM.Color != null && mVM.Color.Count > 0)
                mVM.ProductImages = _productImageRepo.Get(id, mVM.Color.FirstOrDefault().Id);

            if (mVM.ProductPrice != null && mVM.ProductPrice.Id > 0)
                mVM.ProductPrice.DiscountedPrice = Convert.ToDecimal(Math.Round(Convert.ToDouble(mVM.ProductPrice.SellPrice - (mVM.ProductPrice.SellPrice * mVM.ProductPrice.DiscountPercentage / 100)), 2));

            if (mVM.ProductImages != null && mVM.ProductImages.Count > 0)
            {
                foreach (var item in mVM.ProductImages)
                {
                    if (item.FileName.IsNullOrEmpty())
                        item.Base64String = StaticMethods.GetImageBase64(HostingEnvironment.MapPath("~/Images/No-Image-Found.jpg"));
                    else
                        item.Base64String = StaticMethods.GetImageBase64(HostingEnvironment.MapPath($"~/Images/{item.FileName}"));
                }
            }

            return mVM;
        }
    }
}
