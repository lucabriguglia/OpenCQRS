using Microsoft.AspNetCore.Builder;

namespace Kledex.Extensions
{
    public interface IKledexAppBuilder
    {
        IApplicationBuilder App { get; }
    }
}