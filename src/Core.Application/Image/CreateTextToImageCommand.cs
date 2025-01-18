using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Image;
using Microsoft.SemanticKernel.TextToImage;
using System.Text;

namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class CreateTextToImageCommand : IRequest<TextImageDto>
{
    public Guid Id { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
}

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
public class CreateTextToImageCommandHandler(ITextToImageService imageService, ISemanticKernelContext context, IMapper mapper)
    : IRequestHandler<CreateTextToImageCommand, TextImageDto>
{
    private readonly ITextToImageService _imageService = imageService;
    private readonly IMapper _mapper = mapper;
    private readonly ISemanticKernelContext _context = context;

    public async Task<TextImageDto> Handle(CreateTextToImageCommand request, CancellationToken cancellationToken)
    {

        GuardAgainstEmptyPrompt(request?.Prompt);
        GuardAgainstIdExsits(_context.TextImages, request!.Id);

        // Get response
        var response = await _imageService.GenerateImageAsync(description: request.Prompt, width: request.Width, height: request.Height, cancellationToken: cancellationToken);


        // Handle response containing rather a Uri or a Base64 byte array
        Uri.TryCreate(response, UriKind.Absolute, out var returnUri);

        // Persist chat session
        var textImage = new TextImageEntity()
        {
            Id = request.Id == Guid.Empty ? Guid.NewGuid() : request.Id,
            Description = request.Prompt,
            Width = request.Width,
            Height = request.Height,
            ImageBytes = Encoding.UTF8.GetBytes(response),
            ImageUrl = returnUri
        };
        _context.TextImages.Add(textImage);
        await _context.SaveChangesAsync(cancellationToken);

        // Return session
        TextImageDto returnValue;
        try
        {
            returnValue = _mapper.Map<TextImageDto>(textImage);
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

    private static void GuardAgainstIdExsits(DbSet<TextImageEntity> dbSet, Guid id)
    {
        if (dbSet.Any(x => x.Id == id))
            throw new CustomValidationException(
            [
                new("Id", "Id already exists")
            ]);
    }
}
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.