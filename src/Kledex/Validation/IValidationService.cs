using Kledex.Commands;
using System.Threading.Tasks;

namespace Kledex.Validation
{
    public interface IValidationService
    {
        Task ValidateAsync(ICommand command);
    }
}
