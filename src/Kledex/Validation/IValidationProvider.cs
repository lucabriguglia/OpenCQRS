using Kledex.Commands;
using System.Threading.Tasks;

namespace Kledex.Validation
{
    public interface IValidationProvider
    {
        Task<ValidationResponse> ValidateAsync(ICommand command);
        ValidationResponse Validate(ICommand command);
    }
}
