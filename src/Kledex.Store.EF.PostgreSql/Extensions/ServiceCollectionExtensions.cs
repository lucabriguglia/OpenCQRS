using System;
using Kledex.Extensions;
using Kledex.Store.EF.Configuration;
using Kledex.Store.EF.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Store.EF.PostgreSql.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddPostgreSqlStore(this IKledexServiceBuilder builder)
        {
            return AddPostgreSqlStore(builder, opt => { });
        }

        public static IKledexServiceBuilder AddPostgreSqlStore(this IKledexServiceBuilder builder, Action<DatabaseOptions> configureOptions)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (configureOptions == null)
                throw new ArgumentNullException(nameof(configureOptions));

            builder.AddEFStore();

            builder.Services.AddTransient<IDatabaseProvider, PostgreSqlDatabaseProvider>();

            return builder;
        }
    }
}
