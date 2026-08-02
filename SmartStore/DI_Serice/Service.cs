using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SmartStore.Repositories;
using SmartStore.Utilities.DBInitilizer;

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
            services.AddScoped<IDBInitilizer, DBInitilizer>();
        }
    }
}
