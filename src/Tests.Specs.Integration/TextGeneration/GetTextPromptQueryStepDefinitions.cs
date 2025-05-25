using Goodtocode.SemanticKernel.Core.Application.TextGeneration;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Specs.Integration.TextGeneration;

[Binding]
[Scope(Tag = "getTextPromptQuery")]
public class GetTextPromptQueryStepDefinitions : TestBase
{
    private Guid _id;
    private bool _exists;
    private TextPromptDto? _response;

    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        base.def = def;
    }

    [Given(@"I have a text prompt id ""([^""]*)""")]
    public void GivenIHaveATextPromptKey(string textPromptKey)
    {
        if (string.IsNullOrWhiteSpace(textPromptKey)) return;
        Guid.TryParse(textPromptKey, out _id).Should().BeTrue();
    }

    [Given(@"I the text prompt exists ""([^""]*)""")]
    public void GivenITheTextPromptExists(string exists)
    {
        bool.TryParse(exists, out _exists).Should().BeTrue();
    }

    [When(@"I get a text prompt")]
    public async Task WhenIGetATextPrompt()
    {
        if (_exists)
        {
            var textPrompt = TextPromptEntity.Create(_id, Guid.Empty, "Tell me a bedtime story", DateTime.UtcNow);
            textPrompt.TextResponses =
                [
                    TextResponseEntity.Create(Guid.Empty, textPrompt.Id, "Once upon a time...")
                ];
            context.TextPrompts.Add(textPrompt);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetTextPromptQuery()
        {
            Id = _id
        };

        var validator = new GetTextPromptQueryValidator();
        validationResponse = validator.Validate(request);
        if (validationResponse.IsValid)
            try
            {
                var handler = new GetTextPromptQueryHandler(context, Mapper);
                _response = await handler.Handle(request, CancellationToken.None);
                responseType = CommandResponseType.Successful;
            }
            catch (Exception e)
            {
                responseType = HandleAssignResponseType(e);
            }
        else
            responseType = CommandResponseType.BadRequest;
    }

    [Then(@"The response is ""([^""]*)""")]
    public void ThenTheResponseIs(string response)
    {
        HandleHasResponseType(response);
    }

    [Then(@"If the response has validation issues I see the ""([^""]*)"" in the response")]
    public void ThenIfTheResponseHasValidationIssuesISeeTheInTheResponse(string expectedErrors)
    {
        HandleExpectedValidationErrorsAssertions(expectedErrors);
    }

    [Then(@"If the response is successful the response has a Key")]
    public void ThenIfTheResponseIsSuccessfulTheResponseHasAKey()
    {
        if (responseType != CommandResponseType.Successful) return;
        _response?.Id.Should().NotBeEmpty();
    }
}
