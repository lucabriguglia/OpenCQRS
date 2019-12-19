using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using AutoMapper;
using Kledex.Domain;
using Kledex.Events;
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
                    var commandTypes = type.Assembly.GetTypes()
                        .Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && typeof(ICommand).IsAssignableFrom(t))
                        .ToList();

                    foreach (var commandType in commandTypes)
                    {
                        cfg.CreateMap(commandType, commandType);
                    }

                    var eventTypes = type.Assembly.GetTypes()
                        .Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && typeof(IEvent).IsAssignableFrom(t))
                        .ToList();

                    foreach (var eventType in eventTypes)
                    {
                        cfg.CreateMap(eventType, eventType);
                    }
                }
            });

            services.AddSingleton(sp => autoMapperConfig.CreateMapper());

            return services;
        }
    }
}
