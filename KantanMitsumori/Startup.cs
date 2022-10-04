using KantanMitsumori.DataAccess;
using KantanMitsumori.Helper.CommonFuncs;
using KantanMitsumori.Infrastructure;
using KantanMitsumori.Service;
using Microsoft.EntityFrameworkCore;
namespace KantanMitsumori
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<ASESTContext>(
                options => options.UseSqlServer(
                Configuration.GetConnectionString("AsestConnection"))

                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()

            );
            services.AddDbContext<IDEContext>(
                options => options.UseSqlServer(
                Configuration.GetConnectionString("IDEConnection"))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()

            );
            HelperToken.Configure(Configuration);
            CommonSettings.Configure(Configuration);
            services.AddUnitOfWork();
            services.AddHttpClient();
            services.AddBusinessServices();
            services.AddHelperServices();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddLog4Net("log4net.config");

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();
            //app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
