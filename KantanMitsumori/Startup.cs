using KantanMitsumori.Attribute;
using KantanMitsumori.DataAccess;
using KantanMitsumori.Helper;
using KantanMitsumori.Helper.Constant;
using KantanMitsumori.Helper.Settings;
using KantanMitsumori.Infrastructure;
using KantanMitsumori.Service;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

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
            //services.AddWebOptimizer(minifyJavaScript: false, minifyCss: false);
            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddJavaScriptBundle("/js/bundle.js", "/lib/jquery/dist/jquery.js", "/lib/jquery-validation/dist/jquery.validate.js",
                         "/js/moment.min.js", "/js/estcommon.js", "/js/framework.js", "/js/Funcmoment.js", "lib/jquery-validation/dist/additional-methods.js",
                         "/js/jquery.twbsPagination.js", "lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js",
                         "/lib/MicrosoftAjax/MicrosoftAjax.js", "lib/bootstrap/dist/js/bootstrap.js");
                pipeline.AddJavaScriptBundle("/js/bundleEstmain.js", "js/punycode.js", "/js/strutil.js", "js/EstMain.js");
                pipeline.MinifyCssFiles();
                pipeline.MinifyJsFiles();                
            });

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
            services.AddUnitOfWork();
            services.AddHttpClient();           
            services.AddDistributedMemoryCache();       
            var sessionTimeOut = Convert.ToInt32(Configuration.GetValue<string>("Settings:SessionTimeOut"));
            services.AddSession(options =>
            {
                options.Cookie.Name = ".KantanMitsumori.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeOut);
                options.Cookie.IsEssential = true;
            });
            services.AddBusinessServices();
            services.AddHelperServices();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<DataSettings>(Configuration.GetSection("DataSettings"));
            services.Configure<PhysicalPathSettings>(Configuration.GetSection("PhysicalPathSettings"));
            services.Configure<TestSettings>(Configuration.GetSection("TestSettings"));
            services.Configure<URLSettings>(Configuration.GetSection("URLSettings"));
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseWebOptimizer();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();
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
