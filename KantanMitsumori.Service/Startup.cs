using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.IService;
using KantanMitsumori.Service.ASEST;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Service
{
    public static class Startup
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddTransient<IAppService, AppService>();
            services.AddTransient<IEstimateIdeService, EstimateIdeService>();
            services.AddTransient<IEstimateService, EstimateService>();
            services.AddTransient<IEstimateSubService, EstimateSubService>();    
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IInpLoanService, InpLoanService>();
            services.AddTransient<ISelCarService, SelCarService>();
            services.AddTransient<IInpZeiHokenService, InpZeiHokenService>();
            services.AddTransient<IInpInitValService, InpInitValService>();
            return services;
        }

        public static IServiceCollection AddAuthenService(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddHelperServices(this IServiceCollection services)
        { 
            services.AddScoped<HelperMapper>();
            services.AddScoped<CommonFuncHelper>();
            return services;
        }
    }
}
