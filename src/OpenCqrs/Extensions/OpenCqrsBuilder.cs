using System;
using Microsoft.Extensions.DependencyInjection;

namespace OpenCqrs.Extensions
{
    public class OpenCqrsBuilder : IOpenCqrsBuilder
    {
        public IServiceCollection Services { get; }

        public OpenCqrsBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }
    }
}