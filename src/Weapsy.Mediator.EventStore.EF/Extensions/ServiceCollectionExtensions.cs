using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Mediator.EventStore.EF.Configuration;

// ReSharper disable InconsistentNaming

namespace Weapsy.Mediator.EventStore.EF.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyMediatorEFOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<MediatorData>(c => {
                c.EFProvider = (DataProvider)Enum.Parse(
                    typeof(DataProvider),
                    configuration.GetSection("MediatorData")["EFProvider"]);
            });

            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));

            return services;
        }

        public static IServiceCollection AddWeapsyMediatorEF(this IServiceCollection services, IConfiguration configuration)
        {
            services.Scan(s => s
                .FromAssembliesOf(typeof(MediatorDbContext))
                .AddClasses()
                .AsImplementedInterfaces());

            var dataProviderConfig = configuration.GetSection("MediatorData")["EFProvider"];
            var connectionStringConfig = configuration.GetConnectionString("MediatorConnection");

            var currentAssembly = typeof(ServiceCollectionExtensions).GetTypeInfo().Assembly;
            var allDataProviders = currentAssembly.GetImplementationsOf<IDataProvider>();

            var configuredDataProvider = allDataProviders.SingleOrDefault(x => x.Provider.ToString() == dataProviderConfig);

            if (configuredDataProvider == null)
                throw new ApplicationException("The Mediator EF Data Provider entry in appsettings.json is empty or the one specified has not been found.");

            configuredDataProvider.RegisterDbContext(services, connectionStringConfig);

            return services;
        }

        private static IEnumerable<T> GetImplementationsOf<T>(this Assembly assembly)
        {
            var result = new List<T>();

            var types = assembly.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && typeof(T).IsAssignableFrom(t))
                .ToList();

            foreach (var type in types)
            {
                var instance = (T)Activator.CreateInstance(type);
                result.Add(instance);
            }

            return result;
        }
    }
}
