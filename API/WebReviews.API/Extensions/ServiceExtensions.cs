using Contracts;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Service.Contracts;

namespace WebReviews.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDataBase(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<WebReviewsContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();

        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });      
    }
}
