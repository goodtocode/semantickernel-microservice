using Goodtocode.SemanticKernel.Core.Application.Image;
using Goodtocode.SemanticKernel.Core.Domain.Image;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text;

namespace Goodtocode.SemanticKernel.Specs.Integration.Image;

[Binding]
[Scope(Tag = "createTextToImageCommand")]
public class CreateTextToImageCommandStepDefinitions : TestBase
{
    private string _prompt = string.Empty;
    private Guid _id;
    private bool _exists;

    [Given(@"I have a def ""([^""]*)""")]
    public void GivenIHaveADef(string def)
    {
        _def = def;
    }

    [Given(@"I have a initial prompt ""([^""]*)""")]
    public void GivenIHaveAInitialprompt(string prompt)
    {
        _prompt = prompt;
    }

    [Given(@"I have a text image id ""([^""]*)""")]
    public void GivenIHaveATextImageKey(string id)
    {
        _id = Guid.Parse(id);
    }

    [Given(@"The text image exists ""([^""]*)""")]
    public void GivenThetextImageExists(string exists)
    {
        _exists = bool.Parse(exists);
    }

    [When(@"I create a text image with the prompt")]
    public async Task WhenICreateATextImageWithTheprompt()
    {
        // Setup the database if want to test existing records
        if (_exists)
        {
            var textImage = new TextImageEntity()
            {
                Id = Guid.NewGuid(),
                Description = "Image of a simple geometric design consisting of two yellow squares and one blue square. " +
                    "The blue square is placed at a 45-degree angle, positioned centrally below the two yellow squares, creating a symmetrical arrangement. " +
                    "Each square is connected by what appears to be black lines or sticks, suggesting they may represent nodes or elements in a network or structure. " +
                    "The background is white, which contrasts with the bright colors of the squares.",
                Width = 1024,
                Height = 1024,
                ImageBytes = [0x01, 0x02, 0x03, 0x04],
                Timestamp = DateTime.UtcNow
            };
            _context.TextImages.Add(textImage);
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        // Test command
        var request = new CreateTextToImageCommand()
        {
            Id = _id,
            Prompt = _prompt,
            Width = 1024,
            Height = 1024
        };

        var validator = new CreateTextToImageCommandValidator();
        _validationResponse = await validator.ValidateAsync(request);

        if (_validationResponse.IsValid)
        {
            try
            {
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                var imageService = new OpenAITextToImageService(modelId: _optionsOpenAi.ImageModelId, apiKey: _optionsOpenAi.ApiKey);
#pragma warning restore SKEXP0010
                var handler = new CreateTextToImageCommandHandler(imageService, _context, Mapper);
                await handler.Handle(request, CancellationToken.None);
                _responseType = CommandResponseType.Successful;
            }
            catch (Exception e)
            {
                HandleAssignResponseType(e);
            }
        }
        else
            _responseType = CommandResponseType.BadRequest;
    }

    [Then(@"I see the text image created with the initial response ""([^""]*)""")]
    public void ThenISeeTheTextImageCreatedWithTheInitialResponse(string response)
    {
        HandleHasResponseType(response);
    }

    [Then(@"if the response has validation issues I see the ""([^""]*)"" in the response")]
    public void ThenIfTheResponseHasValidationIssuesISeeTheInTheResponse(string expectedErrors)
    {
        HandleExpectedValidationErrorsAssertions(expectedErrors);
    }
}
