using System.Collections.Generic;
using Kledex.Queries;
using Kledex.Sample.NoEventSourcing.Domain;

namespace Kledex.Sample.NoEventSourcing.Reporting
{
    public class GetPublishedProducts : Query<IEnumerable<Product>>
    {
    }
}
