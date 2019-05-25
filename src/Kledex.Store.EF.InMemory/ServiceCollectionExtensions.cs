using System;
using Kledex.Extensions;
using Kledex.Store.EF.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Store.EF.InMemory
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddInMemoryProvider(this IKledexServiceBuilder builder, IConfiguration configuration)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            builder.AddEFProvider(configuration);

            builder.Services.AddDbContext<DomainDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "DomainDb"));

            builder.Services.AddTransient<IDatabaseProvider, InMemoryDatabaseProvider>();

            return builder;
        }
    }
}
