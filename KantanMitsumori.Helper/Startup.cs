using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Settings;
using Microsoft.Extensions.DependencyInjection;
namespace KantanMitsumori.Helper
{
    public static class Startup
    {
        public static IServiceCollection AddHelperServices(this IServiceCollection services)
        {
            services.AddScoped<HelperMapper>();
            //services.AddScoped<CommonSettings>();
            services.AddScoped<TestSettings>();
            services.AddScoped<DataSettings>();
            services.AddScoped<PhysicalPathSettings>();
            services.AddScoped<URLSettings>();
            services.AddScoped<ConnectionStrings>();
            services.AddScoped<JwtSettings>();
            return services;
        }
    }
}
