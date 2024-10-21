using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Prof.Hub.SharedKernel.Results;

namespace Prof.Hub.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validationResults = await ValidateRequestAsync(request, cancellationToken);
            if (validationResults is not null)
            {
                return HandleValidationFailures(validationResults);
            }

            return await next();
        }

        private async Task<List<ValidationError>?> ValidateRequestAsync(TRequest request, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return null;

            var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(request, cancellationToken)));
            var validationErrors = results.SelectMany(ExtractErrors).ToList();

            return validationErrors.Count > 0 ? validationErrors : null;
        }

        private List<ValidationError> ExtractErrors(ValidationResult validationResult)
        {
            return validationResult.Errors
                .Where(failure => failure is not null)
                .Select(failure => new ValidationError
                {
                    ErrorMessage = failure.ErrorMessage,
                    ErrorCode = failure.ErrorCode,
                    Identifier = failure.PropertyName
                })
                .ToList();
        }

        private TResponse HandleValidationFailures(List<ValidationError> errors)
        {
            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = typeof(TResponse).GetGenericArguments()[0];
                var invalidMethod = typeof(Result<>)
                    .MakeGenericType(resultType)
                    .GetMethod(nameof(Result<int>.Invalid), new[] { typeof(List<ValidationError>) });

                if (invalidMethod is not null)
                {
                    return (TResponse)invalidMethod.Invoke(null, new object[] { errors });
                }
            }
            else if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Invalid(errors);
            }
            else
            {
                throw new ValidationException(errors.Select(e => new ValidationFailure(e.Identifier, e.ErrorMessage)));
            }

            throw new InvalidOperationException("Tipo de resposta inesperado.");
        }
    }
}
