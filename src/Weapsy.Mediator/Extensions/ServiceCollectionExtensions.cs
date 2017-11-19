using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Weapsy.Mediator. 
        /// Pass any of the types that are contained in the assemblies where your messages and handlers are.
        /// One for each assembly.
        /// e.g. typeOf(CreatePost) where CreatePost is one of your commands.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="types">The types.</param>
        public static void AddWeapsyMediator(this IServiceCollection services, params Type[] types)
        {
            // Convert to list and add IMediator.
            var typeList = types.ToList();
            typeList.Add(typeof(IMediator));

            // Use Scrutor to register services
            services.Scan(s => s
                .FromAssembliesOf(typeList)
                .AddClasses()
                .AsImplementedInterfaces());

            // Register repository
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}