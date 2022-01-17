using System;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Extensions;

namespace OpenCqrs.Validation.FluentValidation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddFluentValidation(this IKledexServiceBuilder builder)
        {
            return AddFluentValidation(builder, opt => { });
        }

        public static IKledexServiceBuilder AddFluentValidation(this IKledexServiceBuilder builder, Action<ValidationOptions> configureOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            builder.Services.Configure(configureOptions);

            builder.Services.AddTransient<IValidationProvider, FluentValidationProvider>();

            return builder;
        }
    }
}
