using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Audio;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.TextToAudio;

namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class CreateTextToAudioCommand : IRequest<TextAudioDto>
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public string Prompt { get; set; } = string.Empty;
}

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
public class CreateTextToAudioCommandHandler(Kernel kernel, ISemanticKernelContext context) : IRequestHandler<CreateTextToAudioCommand, TextAudioDto>
{
    private readonly Kernel _kernel = kernel;
    private readonly ISemanticKernelContext _context = context;

    public async Task<TextAudioDto> Handle(CreateTextToAudioCommand request, CancellationToken cancellationToken)
    {
        GuardAgainstMissingAuthor(request.AuthorId);
        GuardAgainstEmptyPrompt(request?.Prompt);
        GuardAgainstIdExsits(_context.TextAudio, request!.Id);

        var service = _kernel.GetRequiredService<ITextToAudioService>();
        var executionSettings = new PromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
        var response = await service.GetAudioContentAsync(request.Prompt, executionSettings, _kernel, cancellationToken);

        var textAudio = TextAudioEntity.Create(request.Id, request.AuthorId, request.Prompt, response.Data.GetValueOrDefault().ToArray(), response.Uri);
        _context.TextAudio.Add(textAudio);
        await _context.SaveChangesAsync(cancellationToken);

        return TextAudioDto.CreateFrom(textAudio);
    }

    private static void GuardAgainstMissingAuthor(Guid authorId)
    {
        if (authorId == Guid.Empty)
            throw new CustomValidationException(
            [
                new("AuthorId", "AuthorId required for sessions")
            ]);
    }

    private static void GuardAgainstEmptyPrompt(string? prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            throw new CustomValidationException(
            [
                new("Prompt", "A prompt is required to get a response")
            ]);
    }

    private static void GuardAgainstIdExsits(DbSet<TextAudioEntity> dbSet, Guid id)
    {
        if (dbSet.Any(x => x.Id == id))
            throw new CustomValidationException(
            [
                new("Id", "Id already exists")
            ]);
    }
}
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.