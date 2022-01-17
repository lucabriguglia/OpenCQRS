using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kledex.Queries;
using Kledex.Sample.EventSourcing.Domain;
using Kledex.Sample.EventSourcing.Reporting.Data;
using Kledex.Sample.EventSourcing.Reporting.Queries;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.EventSourcing.Reporting.QueryHandlers
{
    public class GetPublishedProductsHandler : IQueryHandlerAsync<GetPublishedProducts, IEnumerable<ProductEntity>>
    {
        private readonly ReportingDbContext _dbContext;

        public GetPublishedProductsHandler(ReportingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ProductEntity>> HandleAsync(GetPublishedProducts query)
        {
            return await _dbContext.Products.Where(x => x.Status == ProductStatus.Published).ToListAsync();
        }
    }
}
