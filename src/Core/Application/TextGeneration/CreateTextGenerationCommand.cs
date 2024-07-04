﻿using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;
using Microsoft.SemanticKernel.TextGeneration;

namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class CreateTextPromptCommand : IRequest<TextPromptDto>
{
    public Guid Id { get; set; }
    public string? Prompt { get; set; }
}

public class CreateTextPromptCommandHandler(ITextGenerationService textService, ISemanticKernelContext context, IMapper mapper)
    : IRequestHandler<CreateTextPromptCommand, TextPromptDto>
{
    private readonly ITextGenerationService _textService = textService;
    private readonly IMapper _mapper = mapper;
    private readonly ISemanticKernelContext _context = context;

    public async Task<TextPromptDto> Handle(CreateTextPromptCommand request, CancellationToken cancellationToken)
    {

        GuardAgainstEmptyPrompt(request?.Prompt);
        GuardAgainstIdExsits(_context.TextPrompts, request!.Id);

        // Get response
        var responses = await _textService.GetTextContentsAsync(request.Prompt, null, null, cancellationToken);

        // Persist chat session
        var textPrompt = new TextPromptEntity()
        {
            Id = request.Id == Guid.Empty ? Guid.NewGuid() : request.Id,
            Prompt = request.Prompt
        };
        foreach(var response in responses)
        {            
            textPrompt.TextResponses.Add(new TextResponseEntity()
            {
                Response = response.ToString(),
                Timestamp = DateTime.UtcNow
            });
        }
        _context.TextPrompts.Add(textPrompt);
        await _context.SaveChangesAsync(cancellationToken);

        // Return session
        TextPromptDto returnValue;
        try
        {
            returnValue = _mapper.Map<TextPromptDto>(textPrompt);
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

    private static void GuardAgainstIdExsits(DbSet<TextPromptEntity> dbSet, Guid id)
    {
        if (dbSet.Any(x => x.Id == id))
            throw new CustomValidationException(
            [
                new("Id", "Id already exists")
            ]);
    }
}