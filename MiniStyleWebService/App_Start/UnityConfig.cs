using MiniStyleWebService.Infrastructure.IRepository;
using MiniStyleWebService.Infrastructure.IService;
using MiniStyleWebService.Repository;
using MiniStyleWebService.Service;
using System.Web.Http;
using Unity.WebApi;
using Unity;

namespace MiniStyleWebService
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            //Service
            container.RegisterType<IAgeGroupService, AgeGroupService>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<ICartService, CartService>();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IChangePasswordService, ChangePasswordService>();
            container.RegisterType<IColorService, ColorService>();
            container.RegisterType<IEmailSenderService, EmailSenderService>();
            container.RegisterType<IFabricService, FabricService>();
            container.RegisterType<IFileReaderService, FileReaderService>();
            container.RegisterType<IForgotPasswordService, ForgotPasswordService>();
            container.RegisterType<IJWTokenService, JWTokenService>();
            container.RegisterType<IMicroCategoryService, MicroCategoryService>();
            container.RegisterType<IOrderLineService, OrderLineService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IProductAgeGroupService, ProductAgeGroupService>();
            container.RegisterType<IProductColorService, ProductColorService>();
            container.RegisterType<IProductDetailService, ProductDetailService>();
            container.RegisterType<IProductImageService, ProductImageService>();
            container.RegisterType<IProductPriceService, ProductPriceService>();
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<IProductSizeChartService, ProductSizeChartService>();
            container.RegisterType<ISubCategoryService, SubCategoryService>();
            container.RegisterType<IUserService, UserService>();

            //Repository
            container.RegisterType<IAgeGroupRepository, AgeGroupRepository>();
            container.RegisterType<ICartRepository, CartRepository>();
            container.RegisterType<ICategoryRepository, CategoryRepository>();
            container.RegisterType<IColorRepository, ColorRepository>();
            container.RegisterType<IFabricRepository, FabricRepository>();
            container.RegisterType<IMicroCategoryRepository, MicroCategoryRepository>();
            container.RegisterType<IOrderLineRepository, OrderLineRepository>();
            container.RegisterType<IOrderRepository, OrderRepository>();
            container.RegisterType<IProductAgeGroupRepository, ProductAgeGroupRepository>();
            container.RegisterType<IProductColorRepository, ProductColorRepository>();
            container.RegisterType<IProductDetailRepository, ProductDetailRepository>();
            container.RegisterType<IProductImageRepository, ProductImageRepository>();
            container.RegisterType<IProductPriceRepository, ProductPriceRepository>();
            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IProductSizeChartRepository, ProductSizeChartRepository>();
            container.RegisterType<ISubCategoryRepository, SubCategoryRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}