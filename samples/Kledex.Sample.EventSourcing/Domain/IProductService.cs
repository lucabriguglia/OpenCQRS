using Kledex.Commands;
using Kledex.Sample.EventSourcing.Domain.Commands;
using System.Threading.Tasks;

namespace Kledex.Sample.EventSourcing.Domain
{
    public interface IProductService
    {
        Task<CommandResponse> CreateProductAsync(CreateProduct command);
    }
}
