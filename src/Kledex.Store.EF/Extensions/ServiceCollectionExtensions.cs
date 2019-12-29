using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

// ReSharper disable InconsistentNaming

namespace Kledex.Store.EF.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddEFStore(this IKledexServiceBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Scan(s => s
                .FromAssembliesOf(typeof(DomainDbContext))
                .AddClasses()
                .AsImplementedInterfaces());

            return builder;
        }
    }
}
