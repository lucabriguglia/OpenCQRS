using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Extensions
{
    public interface IKledexServiceBuilder
    {
        IServiceCollection Services { get; }
    }
}
