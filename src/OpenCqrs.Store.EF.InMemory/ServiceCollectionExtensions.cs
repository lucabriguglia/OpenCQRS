using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Extensions;
using OpenCqrs.Store.EF.Extensions;

namespace OpenCqrs.Store.EF.InMemory
{
    public static class ServiceCollectionExtensions
    {
        public static IOpenCqrsServiceBuilder AddInMemoryProvider(this IOpenCqrsServiceBuilder builder, IConfiguration configuration)
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
