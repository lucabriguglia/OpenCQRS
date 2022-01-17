using Microsoft.Extensions.DependencyInjection;

namespace OpenCqrs.Extensions
{
    public interface IKledexServiceBuilder
    {
        IServiceCollection Services { get; }
    }
}
