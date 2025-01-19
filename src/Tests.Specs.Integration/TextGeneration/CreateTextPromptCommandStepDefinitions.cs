using Goodtocode.SemanticKernel.Core.Application.TextGeneration;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;
using Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Services;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Goodtocode.SemanticKernel.Specs.Integration.TextGeneration;

[Binding]
[Scope(Tag = "createTextPromptCommand")]
public class CreateTextPromptCommandStepDefinitions : TestBase
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

    [Given(@"I have a text prompt id ""([^""]*)""")]
    public void GivenIHaveATextPromptKey(string id)
    {
        _id = Guid.Parse(id);
    }

    [Given(@"The text prompt exists ""([^""]*)""")]
    public void GivenTheTextPromptExists(string exists)
    {
        _exists = bool.Parse(exists);
    }

    [When(@"I create a text prompt with the prompt")]
    public async Task WhenICreateATextPromptWithTheprompt()
    {
        // Setup the database if want to test existing records
        if (_exists)
        {
            var textPrompt = new TextPromptEntity()
            {
                Id = _id,
                Prompt = _prompt,
                TextResponses =
                 [
                     new TextResponseEntity()
                     {
                         Response = "Fantastic story here.",
                         Timestamp = DateTime.Now
                     }
                 ],
                Timestamp = DateTime.UtcNow,
            };
            context.TextPrompts.Add(textPrompt);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // Test command
        var request = new CreateTextPromptCommand()
        {
            Id = _id,
            Prompt = _prompt
        };

        var validator = new CreateTextPromptCommandValidator();
        validationResponse = await validator.ValidateAsync(request);

        if (validationResponse.IsValid)
        {
            try
            {
                var textService = new TextGenerationService(new OpenAIChatCompletionService(optionsOpenAi.ChatCompletionModelId, optionsOpenAi.ApiKey));
                var handler = new CreateTextPromptCommandHandler(textService, context, Mapper);
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

    [Then(@"I see the text prompt created with the initial response ""([^""]*)""")]
    public void ThenISeeTheTextPromptCreatedWithTheInitialResponse(string response)
    {
        HandleHasResponseType(response);
    }

    [Then(@"if the response has validation issues I see the ""([^""]*)"" in the response")]
    public void ThenIfTheResponseHasValidationIssuesISeeTheInTheResponse(string expectedErrors)
    {
        HandleExpectedValidationErrorsAssertions(expectedErrors);
    }
}
