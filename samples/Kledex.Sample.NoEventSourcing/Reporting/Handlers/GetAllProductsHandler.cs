using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kledex.Queries;
using Kledex.Sample.NoEventSourcing.Data;
using Kledex.Sample.NoEventSourcing.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.NoEventSourcing.Reporting.Handlers
{
    public class GetAllProductsHandler : IQueryHandlerAsync<GetAllProducts, IList<Product>>
    {
        private readonly SampleDbContext _dbContext;

        public GetAllProductsHandler(SampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Product>> HandleAsync(GetAllProducts query)
        {
            return await _dbContext.Products.Where(x => x.Status != ProductStatus.Deleted).ToListAsync();
        }
    }
}
