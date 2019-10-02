using System.Collections.Generic;
using Kledex.Queries;
using Kledex.Sample.NoEventSourcing.Domain;

namespace Kledex.Sample.NoEventSourcing.Reporting.Products
{
    public class GetAllProducts : IQuery<IList<Product>>
    {
    }
}
