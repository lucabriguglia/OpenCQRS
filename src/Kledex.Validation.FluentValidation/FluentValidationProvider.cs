using System.Collections.Generic;
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

        private ValidationResponse BuildValidationResponse(ValidationResult validationResult)
        {
            var errors = new List<ValidationError>();

            foreach (var failure in validationResult.Errors)
            {
                errors.Add(new ValidationError
                {
                    PropertyName = failure.PropertyName,
                    ErrorMessage = failure.ErrorMessage
                });
            }

            var result = new ValidationResponse
            {
                Errors = errors
            };

            return result;
        }
    }
}
