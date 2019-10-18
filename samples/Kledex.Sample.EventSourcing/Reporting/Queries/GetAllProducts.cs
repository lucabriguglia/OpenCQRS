using System.Collections.Generic;
using Kledex.Queries;
using Kledex.Sample.EventSourcing.Reporting.Data;

namespace Kledex.Sample.EventSourcing.Reporting.Queries
{
    public class GetAllProducts : IQuery<IList<ProductEntity>>
    {
    }
}
