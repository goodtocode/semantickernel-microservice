
namespace Goodtocode.SemanticKernel.Core.Application.Common.Behaviors;

public class CustomValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestDelegateInvoker<TResponse> nextInvoker,
        CancellationToken cancellationToken)
    {
        foreach (var validator in _validators)
        {
            validator.ValidateAndThrow(request);
        }

        return await nextInvoker();
    }
}

public class CustomValidationBehavior<TRequest>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public Task Handle(
        TRequest request,
        RequestDelegateInvoker nextInvoker,
        CancellationToken cancellationToken)
    {
        foreach (var validator in _validators)
        {
            validator.ValidateAndThrow(request);
        }

        // Return the Task from nextInvoker to ensure asynchronous execution and resolve CS1998
        return nextInvoker();
    }
}