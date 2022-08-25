using KantanMitsumori.Infrastructure.Base;
using Microsoft.Extensions.DependencyInjection;


namespace KantanMitsumori.Infrastructure
{
    public static class Startup
    {
        /// <summary>
        /// Add repositories as Scoped lifetime.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWorkIDE, UnitOfWorkIDE>();
            return services;
        }
    }
}
