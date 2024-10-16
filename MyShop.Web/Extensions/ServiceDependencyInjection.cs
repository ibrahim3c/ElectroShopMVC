using MyShop.Services.Interfaces;
using MyShop.Web.Services.Implementations;
using MyShop.Web.Services.Interfaces;

namespace MyShop.Web.Extensions
{
    public static class ServiceDependencyInjection
    {
        public static IServiceCollection AddServiceDependencyInjection(this IServiceCollection services)
        {

            // dependency injection
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IFileService, MyShop.Services.Implementations.FileService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IDashboardService, DashboardService>();
            return services;

        }
    }
}
