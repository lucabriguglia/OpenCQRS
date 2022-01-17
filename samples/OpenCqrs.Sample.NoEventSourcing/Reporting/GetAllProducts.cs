using System.Collections.Generic;
using Kledex.Queries;
using Kledex.Sample.NoEventSourcing.Domain;

namespace Kledex.Sample.NoEventSourcing.Reporting
{
    public class GetAllProducts : Query<IList<Product>>
    {
    }
}
