using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.IService;
using KantanMitsumori.IService.ASEST;
using KantanMitsumori.Service.ASEST;
using KantanMitsumori.Service.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace KantanMitsumori.Service
{
    public static class Startup
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // Add helper services
            services.AddScoped<CommonFuncHelper>();
            services.AddScoped<CommonEstimate>();
            services.AddScoped<CommonFuncHelper>();
            services.AddScoped<CommonIDE>();

            // Add business services
            services.AddTransient<IEstMainService, EstMainService>();
            services.AddTransient<IEstimateIdeService, EstimateIdeService>();
            services.AddTransient<IEstimateService, EstimateService>();            
            services.AddTransient<IEstimateSubService, EstimateSubService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IInpLoanService, InpLoanService>();
            services.AddTransient<ISelCarService, SelCarService>();
            services.AddTransient<IInpZeiHokenService, InpZeiHokenService>();
            services.AddTransient<IInpCustKanaService, InpCustKanaService>();
            services.AddTransient<IInpInitValService, InpInitValService>();
            services.AddTransient<IInpNotesService, InpNotesService>();
            services.AddTransient<IInpSitaCarService, InpSitaCarService>();
            services.AddTransient<IInpSyohiyoService, InpSyohiyoService>();
            services.AddTransient<IPreExaminationService, PreExaminationService>();
            
            // Add generic DI
            services.AddTransient<Dictionary<bool, string>, Dictionary<bool, string>>();
            return services;
        }

    }
}
