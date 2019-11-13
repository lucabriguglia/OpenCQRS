using Kledex.Queries;

namespace Kledex.Tests.Fakes
{
    public class GetSomethingCacheable : CacheableQuery<Something>
    {
        public GetSomethingCacheable()
        {
            CacheKey = "SomethingCached";
        }
    }
}
