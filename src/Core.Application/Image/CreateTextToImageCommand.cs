using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Image;
using Microsoft.SemanticKernel;
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
public class CreateTextToImageCommandHandler(Kernel kernel, ISemanticKernelContext context, IMapper mapper)
    : IRequestHandler<CreateTextToImageCommand, TextImageDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly Kernel _kernel = kernel;
    private readonly ISemanticKernelContext _context = context;

    public async Task<TextImageDto> Handle(CreateTextToImageCommand request, CancellationToken cancellationToken)
    {
        GuardAgainstEmptyPrompt(request?.Prompt);
        GuardAgainstIdExsits(_context.TextImages, request!.Id);

        var service = _kernel.GetRequiredService<ITextToImageService>();
        var response = await service.GenerateImageAsync(description: request.Prompt, width: request.Width, height: request.Height, cancellationToken: cancellationToken);
        // Handle response containing rather a Uri or a Base64 byte array
        Uri.TryCreate(response, UriKind.Absolute, out var returnUri);

        var textImage = TextImageEntity.Create(request.Id, request.Prompt, request.Width, request.Height, Encoding.UTF8.GetBytes(response), returnUri);
        _context.TextImages.Add(textImage);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TextImageDto>(textImage);
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