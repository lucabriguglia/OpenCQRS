using System;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Extensions
{
    public class KledexServiceBuilder : IKledexServiceBuilder
    {
        public IServiceCollection Services { get; }

        public KledexServiceBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }
    }
}