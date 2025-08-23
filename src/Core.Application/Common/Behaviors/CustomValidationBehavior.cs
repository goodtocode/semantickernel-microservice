
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