using Microsoft.AspNetCore.Builder;

namespace OpenCqrs.Extensions
{
    public interface IKledexAppBuilder
    {
        IApplicationBuilder App { get; }
    }
}