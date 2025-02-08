﻿using ValidationException = Goodtocode.SemanticKernel.Core.Application.Common.Exceptions.CustomValidationException;

namespace Goodtocode.SemanticKernel.Core.Application.Common.Behaviors;

public class CustomValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v =>
                    v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Count == 0)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count == 0)
                throw new ValidationException(failures);
        }
        return await next();
    }
}
