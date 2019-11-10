using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Kledex.Commands;
using Kledex.Dependencies;

namespace Kledex.Validation.FluentValidation
{
    public class FluentValidationProvider : IValidationProvider
    {
        private readonly IHandlerResolver _handlerResolver;

        public FluentValidationProvider(IHandlerResolver handlerResolver)
        {
            _handlerResolver = handlerResolver;
        }

        public async Task<ValidationResponse> ValidateAsync(ICommand command)
        {
            var validator = _handlerResolver.ResolveHandler(command, typeof(IValidator<>));
            var validateMethod = validator.GetType().GetMethod("ValidateAsync", new [] { command.GetType(), typeof(CancellationToken) });
            var validationResult = await(Task<ValidationResult>)validateMethod.Invoke(validator, new object[] { command, default(CancellationToken) });

            return BuildValidationResponse(validationResult);
        }

        public ValidationResponse Validate(ICommand command)
        {
            var validator = _handlerResolver.ResolveHandler(command, typeof(IValidator<>));
            var validateMethod = validator.GetType().GetMethod("Validate", new[] { command.GetType(), typeof(CancellationToken) });
            var validationResult = (ValidationResult)validateMethod.Invoke(validator, new object[] { command, default(CancellationToken) });

            return BuildValidationResponse(validationResult);
        }

        private static ValidationResponse BuildValidationResponse(ValidationResult validationResult)
        {
            return new ValidationResponse
            {
                Errors = validationResult.Errors.Select(failure => new ValidationError
                {
                    PropertyName = failure.PropertyName,
                    ErrorMessage = failure.ErrorMessage
                }).ToList()
            };
        }
    }
}
