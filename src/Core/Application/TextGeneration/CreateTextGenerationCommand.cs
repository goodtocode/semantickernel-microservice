using FluentValidation.Results;
using Microsoft.SemanticKernel.TextGeneration;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;

namespace Goodtocode.SemanticKernel.Core.Application.TextGeneration;

public class CreateTextGenerationCommand : IRequest<string>
{
    public string? Message { get; set; }
}

public class CreateTextGenerationCommandHandler : IRequestHandler<CreateTextGenerationCommand, string>
{
    private ITextGenerationService _textService;
    //private readonly ISemanticKernelMicroserviceContext _context;

    public CreateTextGenerationCommandHandler(ITextGenerationService textService)//, ISemanticKernelMicroserviceContext context)
    {
        //_context = context;
        _textService = textService;
    }

    public async Task<string> Handle(CreateTextGenerationCommand request, CancellationToken cancellationToken)
    {

        GuardAgainstEmptyMessage(request?.Message);
        var response = await _textService.GetTextContentAsync(request.Message);

        //Id, chatcmpl-9Te5QEaE2fBhxt1mtHamj7U25NIRz
        //{ [Created, { 5/27/2024 11:30:32 PM +00:00}]}
        //ModelId "gpt-3.5-turbo" string
        //Role    { assistant}
        //Microsoft.SemanticKernel.TextGeneration.AuthorRole
        //Content "There are 25 letters in the sentence \"hi, how many letters in this sentence?\""

        return response.Text;

        //var weatherTextGeneration = _context.TextGenerations.Find(request.Key);
        //GuardAgainstWeatherTextGenerationNotFound(weatherTextGeneration);
        //var weatherTextGenerationValue = TextGenerationValue.Create(request.Key, request.Date, (int)request.TemperatureF, request.Zipcodes);
        //if (weatherTextGenerationValue.IsFailure)
        //    throw new Exception(weatherTextGenerationValue.Error);
        //_context.TextGenerations.Add(new TextGeneration(weatherTextGenerationValue.Value));
        //await _context.SaveChangesAsync(CancellationToken.None);
    }

    private static void GuardAgainstEmptyMessage(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new CustomValidationException(new List<ValidationFailure>
            {
                new("Message", "A message is required to get a response")
            });
    }
}