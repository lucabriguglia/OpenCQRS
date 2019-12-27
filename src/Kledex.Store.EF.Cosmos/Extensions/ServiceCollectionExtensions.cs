using System;
using Kledex.Extensions;
using Kledex.Store.EF.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Store.EF.Cosmos.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddCosmosProvider(this IKledexServiceBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.AddEFProvider();

            builder.Services.AddDbContext<DomainDbContext>(options =>
                options.UseCosmos(
                    "https://localhost:8081",
                    "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                    databaseName: "OrdersDB"));

            builder.Services.AddTransient<IDatabaseProvider, CosmosDatabaseProvider>();

            return builder;
        }
    }
}
