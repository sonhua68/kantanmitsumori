using KantanMitsumori.Helper.CommonFuncs;
using Microsoft.Extensions.DependencyInjection;
namespace KantanMitsumori.Helper
{
    public static class Startup
    {
        public static IServiceCollection AddHelperServices(this IServiceCollection services)
        {
            services.AddScoped<HelperMapper>();
            return services;
        }
    }
}
