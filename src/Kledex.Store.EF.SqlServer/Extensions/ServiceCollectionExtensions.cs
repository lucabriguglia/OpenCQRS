using System;
using Kledex.Extensions;
using Kledex.Store.EF.Configuration;
using Kledex.Store.EF.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Store.EF.SqlServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddSqlServerStoreProvider(this IKledexServiceBuilder builder)
        {
            return AddSqlServerStoreProvider(builder, opt => { });
        }

        public static IKledexServiceBuilder AddSqlServerStoreProvider(this IKledexServiceBuilder builder, Action<DatabaseOptions> configureOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            builder.AddEFProvider();

            builder.Services.AddTransient<IDatabaseProvider, SqlServerDatabaseProvider>();

            return builder;
        }
    }
}
