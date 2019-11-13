using Kledex.Extensions;
using Kledex.Store.EF.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

// ReSharper disable InconsistentNaming

namespace Kledex.Store.EF.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddEFProvider(this IKledexServiceBuilder builder, IConfiguration configuration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            builder.Services.Configure<DomainDbConfiguration>(configuration.GetSection(Constants.DomainDbConfigurationSection));

            builder.Services.Scan(s => s
                .FromAssembliesOf(typeof(DomainDbContext))
                .AddClasses()
                .AsImplementedInterfaces());

            return builder;
        }
    }
}
