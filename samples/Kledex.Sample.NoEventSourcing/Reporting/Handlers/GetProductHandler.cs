using System;
using System.Threading.Tasks;
using Kledex.Queries;
using Kledex.Sample.NoEventSourcing.Data;
using Kledex.Sample.NoEventSourcing.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.NoEventSourcing.Reporting.Handlers
{
    public class GetProductHandler : IQueryHandlerAsync<GetProduct, Product>
    {
        private readonly SampleDbContext _dbContext;

        public GetProductHandler(SampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> HandleAsync(GetProduct query)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == query.ProductId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {query.ProductId}");
            }

            return product;
        }
    }
}
