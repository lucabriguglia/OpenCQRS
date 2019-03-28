using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
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

            var typeList = types.ToList();
            typeList.Add(typeof(IDispatcher));

            services.Scan(s => s
                .FromAssembliesOf(typeList)
                .AddClasses()
                .AsImplementedInterfaces());

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddAutoMapper(typeList);

            return new OpenCqrsServiceBuilder(services);
        }

        public static IOpenCqrsServiceBuilder AddOptions(this IOpenCqrsServiceBuilder builder, Action<Options> setupAction)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));

            builder.Services.Configure(setupAction);

            return builder;
        }
    }
}
