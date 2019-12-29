using System;
using Kledex.Extensions;
using Kledex.Store.EF.Configuration;
using Kledex.Store.EF.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Store.EF.MySql.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddMySqlStore(this IKledexServiceBuilder builder)
        {
            return AddMySqlStore(builder, opt => { });
        }

        public static IKledexServiceBuilder AddMySqlStore(this IKledexServiceBuilder builder, Action<DatabaseOptions> configureOptions)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (configureOptions == null)
                throw new ArgumentNullException(nameof(configureOptions));

            builder.AddEFStore();

            builder.Services.AddTransient<IDatabaseProvider, MySqlDatabaseProvider>();

            return builder;
        }
    }
}
