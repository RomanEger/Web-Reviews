using Microsoft.EntityFrameworkCore;
using Repository;

namespace WebReviews.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDataBase(this IServiceCollection services) =>
            services.AddDbContext<WebReviewsContext>(opt => opt.UseNpgsql("DefaultConnection"));
    }
}
