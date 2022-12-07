using KantanMitsumori.Helper.CommonFuncs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KantanMitsumori.Helper
{
    public static class Startup
    {
        public static IServiceCollection AddHelperServices(this IServiceCollection services)
        {
            services.AddScoped<HelperMapper>();
            //services.AddScoped<HelperLogger>();
            return services;
        }
    }
}
