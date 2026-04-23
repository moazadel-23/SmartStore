using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SmartStore.Repositories;

namespace SmartStore.DI_Serice
{
    public static class Service
    {
        public static void AddScopedServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Product>, Repository<Product>>();
            services.AddScoped<IRepository<Brand>, Repository<Brand>>();
            services.AddScoped<IRepository<Category>, Repository<Category>>();
            services.AddScoped<IRepository<ProductImges>, Repository<ProductImges>>();
            services.AddScoped<IRepository<Order>, Repository<Order>>();
            services.AddScoped<IRepository<OrderItem>, Repository<OrderItem>>();
        }
    }
}
