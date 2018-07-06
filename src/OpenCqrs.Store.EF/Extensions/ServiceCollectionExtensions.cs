using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Extensions;
using OpenCqrs.Store.EF.Configuration;

// ReSharper disable InconsistentNaming

namespace OpenCqrs.Store.EF.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IOpenCqrsServiceBuilder AddEFProvider(this IOpenCqrsServiceBuilder builder, IConfiguration configuration)
        {
            builder.Services.Scan(s => s
                .FromAssembliesOf(typeof(DomainDbContext))
                .AddClasses()
                .AsImplementedInterfaces());

            builder.Services.AddTransient<IDomainDbContext, DomainDbContext>();
            builder.Services.Configure<DomainDbConfiguration>(configuration.GetSection(Constants.DomainDbConfiguration));

            return builder;
        }
    }
}
