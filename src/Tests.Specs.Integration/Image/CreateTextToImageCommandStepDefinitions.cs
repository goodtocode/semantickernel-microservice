using System.Text;
using Goodtocode.SemanticKernel.Core.Application.Image;
using Goodtocode.SemanticKernel.Core.Domain.Image;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

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
        base.def = def;
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
            var textImage = TextImageEntity.Create(Guid.NewGuid(), _prompt, 1024, 1024, new ReadOnlyMemory<byte>([0x01, 0x02, 0x03, 0x04]));
            context.TextImages.Add(textImage);
            await context.SaveChangesAsync(CancellationToken.None);
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
        validationResponse = await validator.ValidateAsync(request);

        if (validationResponse.IsValid)
        {
            try
            {
                var handler = new CreateTextToImageCommandHandler(kernel, context, Mapper);
                await handler.Handle(request, CancellationToken.None);
                responseType = CommandResponseType.Successful;
            }
            catch (Exception e)
            {
                HandleAssignResponseType(e);
            }
        }
        else
            responseType = CommandResponseType.BadRequest;
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
