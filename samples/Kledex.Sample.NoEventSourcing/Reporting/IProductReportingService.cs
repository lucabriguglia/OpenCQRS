using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Sample.NoEventSourcing.Domain;

namespace Kledex.Sample.NoEventSourcing.Reporting
{
    public interface IProductReportingService
    {
        Task<IList<Product>> GetProducts();
        Task<IList<Product>> GetAllProducts();
        Task<Product> GetProduct(Guid productId);
    }
}
