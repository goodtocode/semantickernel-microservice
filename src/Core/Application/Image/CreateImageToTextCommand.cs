using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Image;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ImageToText;
using System.Collections;
using System.Text;

namespace Goodtocode.SemanticKernel.Core.Application.Image;

public class CreateImageToTextCommand : IRequest<TextImageDto>
{
    public Guid Id { get; set; }
    public byte[]? ImageBytes { get; set; }
    public Uri? ImageUrl { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
public class CreateImageToTextCommandHandler(IImageToTextService imageService, ISemanticKernelContext context, IMapper mapper)
    : IRequestHandler<CreateImageToTextCommand, TextImageDto>
{
    private readonly IImageToTextService _imageService = imageService;
    private readonly IMapper _mapper = mapper;
    private readonly ISemanticKernelContext _context = context;

    public async Task<TextImageDto> Handle(CreateImageToTextCommand request, CancellationToken cancellationToken)
    {
        GuardAgainstEmptyImagePrompt(request);
        GuardAgainstIdExsits(_context.TextImages, request!.Id);

        // Get response
        var content = new ImageContent()
        {
            Data = request.ImageBytes,
            DataUri = request?.ImageUrl?.ToString()
        };
        var response = await _imageService.GetTextContentAsync(content: content, cancellationToken: cancellationToken);

        // Persist chat session
        var bytes = request?.ImageBytes != null ? Encoding.UTF8.GetString(request.ImageBytes) : null;
        var textImage = new TextImageEntity()
        {
            Id = request!.Id == Guid.Empty ? Guid.NewGuid() : request.Id,
            Description = response.ToString(),
            Width = request.Width,
            Height = request.Height,
            ImageBytes = bytes,
            ImageUrl = request.ImageUrl
        };
        _context.TextImages.Add(textImage);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TextImageDto>(textImage);
    }

    private static void GuardAgainstEmptyImagePrompt(CreateImageToTextCommand? prompt)
    {
        if ((prompt?.ImageUrl == null) || (prompt?.ImageBytes == null))
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