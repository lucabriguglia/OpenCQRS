using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kledex.Queries;
using Kledex.Sample.EventSourcing.Domain;
using Kledex.Sample.EventSourcing.Reporting.Data;
using Kledex.Sample.NoEventSourcing.Reporting.Queries;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.NoEventSourcing.Reporting.QueryHandlers
{
    public class GetAllProductsHandler : IQueryHandlerAsync<GetAllProducts, IList<ProductEntity>>
    {
        private readonly ReportingDbContext _dbContext;

        public GetAllProductsHandler(ReportingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<ProductEntity>> HandleAsync(GetAllProducts query)
        {
            return await _dbContext.Products.Where(x => x.Status != ProductStatus.Deleted).ToListAsync();
        }
    }
}
