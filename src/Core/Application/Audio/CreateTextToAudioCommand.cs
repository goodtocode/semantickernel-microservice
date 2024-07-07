using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Audio;
using Microsoft.SemanticKernel.TextToAudio;

namespace Goodtocode.SemanticKernel.Core.Application.Audio;

public class CreateTextToAudioCommand : IRequest<TextAudioDto>
{
    public Guid Id { get; set; }
    public string Prompt { get; set; } = string.Empty;
}

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
public class CreateTextToAudioCommandHandler(ITextToAudioService AudioService, ISemanticKernelContext context, IMapper mapper)
    : IRequestHandler<CreateTextToAudioCommand, TextAudioDto>
{
    private readonly ITextToAudioService _AudioService = AudioService;
    private readonly IMapper _mapper = mapper;
    private readonly ISemanticKernelContext _context = context;

    public async Task<TextAudioDto> Handle(CreateTextToAudioCommand request, CancellationToken cancellationToken)
    {

        GuardAgainstEmptyPrompt(request?.Prompt);
        GuardAgainstIdExsits(_context.TextAudio, request!.Id);

        // Get response
        var response = await _AudioService.GetAudioContentAsync(text: request.Prompt, cancellationToken: cancellationToken);

        // Persist chat session
        var textAudio = new TextAudioEntity()
        {
            Id = request.Id == Guid.Empty ? Guid.NewGuid() : request.Id,
            Description = request.Prompt,
            AudioBytes = response.Data.GetValueOrDefault().ToArray(),
            AudioUrl = response.Uri
        };
        _context.TextAudio.Add(textAudio);
        await _context.SaveChangesAsync(cancellationToken);

        // Return session
        TextAudioDto returnValue;
        try
        {
            returnValue = _mapper.Map<TextAudioDto>(textAudio);
        }
        catch (Exception)
        {
            throw new CustomValidationException(
            [
                new("Id", "Id already exists")
            ]);
        }
        return returnValue;
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