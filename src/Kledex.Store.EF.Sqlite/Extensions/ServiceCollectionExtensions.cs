using System;
using Kledex.Extensions;
using Kledex.Store.EF.Configuration;
using Kledex.Store.EF.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Store.EF.Sqlite.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddSqliteStoreProvider(this IKledexServiceBuilder builder)
        {
            return AddSqliteStoreProvider(builder, opt => { });
        }

        public static IKledexServiceBuilder AddSqliteStoreProvider(this IKledexServiceBuilder builder, Action<DatabaseOptions> configureOptions)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (configureOptions == null)
                throw new ArgumentNullException(nameof(configureOptions));

            builder.AddEFProvider();

            builder.Services.AddTransient<IDatabaseProvider, SqliteDatabaseProvider>();

            return builder;
        }
    }
}
