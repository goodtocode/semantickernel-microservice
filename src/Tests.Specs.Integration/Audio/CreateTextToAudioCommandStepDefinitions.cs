using Goodtocode.SemanticKernel.Core.Application.Audio;
using Goodtocode.SemanticKernel.Core.Domain.Audio;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text;

namespace Goodtocode.SemanticKernel.Specs.Integration.Audio;

[Binding]
[Scope(Tag = "createTextToAudioCommand")]
public class CreateTextToAudioCommandStepDefinitions : TestBase
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

    [Given(@"I have a text audio id ""([^""]*)""")]
    public void GivenIHaveATextAudioKey(string id)
    {
        _id = Guid.Parse(id);
    }

    [Given(@"The text audio exists ""([^""]*)""")]
    public void GivenThetextAudioExists(string exists)
    {
        _exists = bool.Parse(exists);
    }

    [When(@"I create a text audio with the prompt")]
    public async Task WhenICreateATextAudioWithTheprompt()
    {
        // Setup the database if want to test existing records
        if (_exists)
        {
            var textAudio = new TextAudioEntity()
            {
                Id = Guid.NewGuid(),
                Description = "Audio of a simple geometric design consisting of two yellow squares and one blue square. " +
                    "The blue square is placed at a 45-degree angle, positioned centrally below the two yellow squares, creating a symmetrical arrangement. " +
                    "Each square is connected by what appears to be black lines or sticks, suggesting they may represent nodes or elements in a network or structure. " +
                    "The background is white, which contrasts with the bright colors of the squares.",
                AudioBytes = [0x01, 0x02, 0x03, 0x04],
                Timestamp = DateTime.UtcNow
            };
            context.TextAudio.Add(textAudio);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // Test command
        var request = new CreateTextToAudioCommand()
        {
            Id = _id,
            Prompt = _prompt
        };

        var validator = new CreateTextToAudioCommandValidator();
        validationResponse = await validator.ValidateAsync(request);

        if (validationResponse.IsValid)
        {
            try
            {
#pragma warning disable SKEXP0010
                var audioService = new OpenAITextToAudioService(modelId: optionsOpenAi.AudioModelId, apiKey: optionsOpenAi.ApiKey);
#pragma warning restore SKEXP0010
                var handler = new CreateTextToAudioCommandHandler(audioService, context, Mapper);
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

    [Then(@"I see the text audio created with the initial response ""([^""]*)""")]
    public void ThenISeeTheTextAudioCreatedWithTheInitialResponse(string response)
    {
        HandleHasResponseType(response);
    }

    [Then(@"if the response has validation issues I see the ""([^""]*)"" in the response")]
    public void ThenIfTheResponseHasValidationIssuesISeeTheInTheResponse(string expectedErrors)
    {
        HandleExpectedValidationErrorsAssertions(expectedErrors);
    }
}
