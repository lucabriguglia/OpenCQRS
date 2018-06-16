using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Store.EF.Extensions;

namespace OpenCqrs.Store.EF.PostgreSql
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenCqrsPostgreSqlProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenCqrsEF(configuration);

            var connectionString = configuration.GetSection(Constants.DomainDbConfigurationConnectionString).Value;

            services.AddDbContext<DomainDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddTransient<IDatabaseProvider, PostgreSqlDatabaseProvider>();

            return services;
        }
    }
}
