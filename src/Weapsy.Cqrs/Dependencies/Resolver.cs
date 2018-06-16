using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Cqrs.Dependencies
{
    public class Resolver : IResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Resolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public T Resolve<T>()
        {
            return _httpContextAccessor.HttpContext.RequestServices.GetService<T>();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return _httpContextAccessor.HttpContext.RequestServices.GetServices<T>();
        }
    }
}
