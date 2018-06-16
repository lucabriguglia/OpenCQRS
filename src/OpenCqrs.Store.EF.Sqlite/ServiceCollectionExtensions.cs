using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Store.EF.Extensions;

namespace OpenCqrs.Store.EF.Sqlite
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenCqrsSqliteProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenCqrsEF(configuration);

            var connectionString = configuration.GetSection(Constants.DomainDbConfigurationConnectionString).Value;

            services.AddDbContext<DomainDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddTransient<IDatabaseProvider, SqliteDatabaseProvider>();

            return services;
        }
    }
}
