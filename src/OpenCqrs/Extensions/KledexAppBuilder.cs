using System;
using Microsoft.AspNetCore.Builder;

namespace OpenCqrs.Extensions
{
    public class KledexAppBuilder : IKledexAppBuilder
    {
        public IApplicationBuilder App { get; }

        public KledexAppBuilder(IApplicationBuilder app)
        {
            App = app ?? throw new ArgumentNullException(nameof(app));
        }
    }
}