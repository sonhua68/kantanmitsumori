using KantanMitsumori.IService;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace KantanMitsumori.Service
{
    public static class Startup
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddTransient<IAppService, AppService>();
            return services;
        }

        public static IServiceCollection AddAuthenService(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddHelpServices(this IServiceCollection services)
        {
            services.AddScoped<CommonFuncHelper>();
            return services;
        }
    }
}
