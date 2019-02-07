using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Abstractions;
using OpenCqrs.Abstractions.Domain;
using OpenCqrs.Configuration;
using OpenCqrs.Domain;

namespace OpenCqrs.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds OpenCqrs. 
        /// Pass any of the types that are contained in the assemblies where your messages and handlers are.
        /// One for each assembly.
        /// e.g. typeOf(CreatePost) where CreatePost is one of your commands.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="types">The types.</param>
        public static IOpenCqrsServiceBuilder AddOpenCqrs(this IServiceCollection services, params Type[] types)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            // Convert to list and add IDispatcher.
            var typeList = types.ToList();
            typeList.Add(typeof(Dispatcher));

            // Use Scrutor to register services
            services.Scan(s => s
                .FromAssembliesOf(typeList)
                .AddClasses()
                .AsImplementedInterfaces());

            // Register dispatcher
            services.AddScoped<IDispatcher, Dispatcher>();

            // Register repository
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            return new OpenCqrsServiceBuilder(services);
        }

        public static IOpenCqrsServiceBuilder AddOptions(this IOpenCqrsServiceBuilder builder, Action<EventOptions> setupAction)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));

            // Configure options
            builder.Services.Configure(setupAction);

            return builder;
        }
    }
}