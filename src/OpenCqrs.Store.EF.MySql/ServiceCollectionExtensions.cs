using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Store.EF.Extensions;

namespace OpenCqrs.Store.EF.MySql
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenCqrsMySqlProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenCqrsEFProvider(configuration);

            var connectionString = configuration.GetSection(Constants.DomainDbConfigurationConnectionString).Value;

            services.AddDbContext<DomainDbContext>(options =>
                options.UseMySQL(connectionString));

            services.AddTransient<IDatabaseProvider, MySqlDatabaseProvider>();

            return services;
        }
    }
}
