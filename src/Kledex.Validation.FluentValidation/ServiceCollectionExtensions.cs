using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Validation.FluentValidation
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddFluentValidation(this IKledexServiceBuilder builder)
        {
            builder.Services.AddTransient<IValidationProvider, FluentValidationProvider>();

            return builder;
        }
    }
}
