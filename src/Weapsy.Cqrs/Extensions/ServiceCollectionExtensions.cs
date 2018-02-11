using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Cqrs.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Weapsy.Cqrs. 
        /// Pass any of the types that are contained in the assemblies where your messages and handlers are.
        /// One for each assembly.
        /// e.g. typeOf(CreatePost) where CreatePost is one of your commands.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="types">The types.</param>
        public static IServiceCollection AddWeapsyCqrs(this IServiceCollection services, params Type[] types)
        {
            // Convert to list and add IMediator.
            var typeList = types.ToList();
            typeList.Add(typeof(IDispatcher));

            // Use Scrutor to register services
            services.Scan(s => s
                .FromAssembliesOf(typeList)
                .AddClasses()
                .AsImplementedInterfaces());

            // Register repository
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}