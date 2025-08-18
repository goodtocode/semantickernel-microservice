using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.TextGeneration;

namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class CreateTextPromptCommand : IRequest<TextPromptDto>
{
    public Guid Id { get; set; }
    public string? Prompt { get; set; }
}

public class CreateTextPromptCommandHandler(Kernel semanticKernel, ISemanticKernelContext context) : IRequestHandler<CreateTextPromptCommand, TextPromptDto>
{
    private readonly Kernel _kernel = semanticKernel;
    private readonly ISemanticKernelContext _context = context;

    public async Task<TextPromptDto> Handle(CreateTextPromptCommand request, CancellationToken cancellationToken)
    {
        GuardAgainstEmptyPrompt(request?.Prompt);
        GuardAgainstIdExsits(_context.TextPrompts, request!.Id);

        var service = _kernel.GetRequiredService<ITextGenerationService>();
        var executionSettings = new PromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
        var responses = await service.GetTextContentsAsync(request.Prompt!, executionSettings, _kernel, cancellationToken);

        var textPrompt = TextPromptEntity.Create(request.Id, Guid.NewGuid(), request.Prompt!);
        foreach (var response in responses)
        {
            textPrompt.TextResponses.Add(TextResponseEntity.Create(Guid.NewGuid(), textPrompt.Id, response.ToString()));
        }
        _context.TextPrompts.Add(textPrompt);
        await _context.SaveChangesAsync(cancellationToken);

        return TextPromptDto.CreateFrom(textPrompt);
    }

    private static void GuardAgainstEmptyPrompt(string? prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            throw new CustomValidationException(
            [
                new("Prompt", "A prompt is required to get a response")
            ]);
    }

    private static void GuardAgainstIdExsits(DbSet<TextPromptEntity> dbSet, Guid id)
    {
        if (dbSet.Any(x => x.Id == id))
            throw new CustomValidationException(
            [
                new("Id", "Id already exists")
            ]);
    }
}