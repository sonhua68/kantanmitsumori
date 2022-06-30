using KantanMitsumori.DataAccess;
using KantanMitsumori.Infrastructure;
using KantanMitsumori.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

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
#if DEBUG
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
#endif
            );


            services.AddUnitOfWork();
            services.AddHttpClient();
            services.AddBusinessServices();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            //app.UseDefaultFiles();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Scripts")),
            //    RequestPath = "/Scripts"
            //});

            //loggerFactory.AddLog4Net("log4net.config");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            //app.UseAuthentication();

            //app.UseAuthorization();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Resources")),
                RequestPath = "/Resources"
            });
            app.UseSession();

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
