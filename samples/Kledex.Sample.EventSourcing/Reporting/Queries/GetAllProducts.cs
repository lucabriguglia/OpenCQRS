using System.Collections.Generic;
using Kledex.Queries;
using Kledex.Sample.EventSourcing.Reporting.Data;

namespace Kledex.Sample.NoEventSourcing.Reporting.Queries
{
    public class GetAllProducts : IQuery<IList<ProductEntity>>
    {
    }
}
