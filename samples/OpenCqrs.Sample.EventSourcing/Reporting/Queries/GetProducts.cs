using System.Collections.Generic;
using Kledex.Queries;
using Kledex.Sample.EventSourcing.Reporting.Data;

namespace Kledex.Sample.EventSourcing.Reporting.Queries
{
    public class GetProducts : CacheableQuery<IList<ProductEntity>>
    {
        public GetProducts()
        {
            CacheKey = CacheKeys.ProductsCacheKey;
            CacheTime = 10;
        }
    }
}
