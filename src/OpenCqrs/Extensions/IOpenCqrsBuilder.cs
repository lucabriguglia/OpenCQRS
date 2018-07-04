using Microsoft.Extensions.DependencyInjection;

namespace OpenCqrs.Extensions
{
    public interface IOpenCqrsBuilder
    {
        IServiceCollection Services { get; }
    }
}
