using System;
using Kledex.Extensions;
using Kledex.Store.EF.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Store.EF.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddSqlServerProvider(this IKledexServiceBuilder builder, IConfiguration configuration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            builder.AddEFProvider();

            var connectionString = configuration.GetConnectionString(Consts.DomainStoreConnectionString);

            builder.Services.AddDbContext<DomainDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddTransient<IDatabaseProvider, SqlServerDatabaseProvider>();

            return builder;
        }
    }
}
