using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Kledex.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services, IEnumerable<Type> types)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                foreach (var type in types)
                {
                    var domainEventTypes = type.Assembly.GetTypes()
                        .Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && typeof(IDomainEvent).IsAssignableFrom(t))
                        .ToList();

                    foreach (var domainEventType in domainEventTypes)
                    {
                        cfg.CreateMap(domainEventType, domainEventType);
                    }
                }
            });

            services.AddSingleton(sp => autoMapperConfig.CreateMapper());

            return services;
        }
    }
}
