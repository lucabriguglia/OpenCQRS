using System;
using Kledex.Commands;
using System.Threading.Tasks;

namespace Kledex.Validation
{
    public interface IValidationProvider
    {
        Task<ValidationResponse> ValidateAsync(ICommand command);
        ValidationResponse Validate(ICommand command);
    }

    public class DefaultValidationProvider : IValidationProvider
    {
        public Task<ValidationResponse> ValidateAsync(ICommand command)
        {
            throw new NotImplementedException(Consts.ValidationRequiredMessage);
        }

        public ValidationResponse Validate(ICommand command)
        {
            throw new NotImplementedException(Consts.ValidationRequiredMessage);
        }
    }
}
