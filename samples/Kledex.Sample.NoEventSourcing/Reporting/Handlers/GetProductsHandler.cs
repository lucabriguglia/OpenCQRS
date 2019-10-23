using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kledex.Queries;
using Kledex.Sample.NoEventSourcing.Data;
using Kledex.Sample.NoEventSourcing.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.NoEventSourcing.Reporting.Handlers
{
    public class GetProductsHandler : IQueryHandlerAsync<GetProducts, IList<Product>>
    {
        private readonly SampleDbContext _dbContext;

        public GetProductsHandler(SampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Product>> HandleAsync(GetProducts query)
        {
            return await _dbContext.Products.Where(x => x.Status != ProductStatus.Deleted).ToListAsync();
        }
    }
}
