using Goodtocode.SemanticKernel.Core.Application.TextGeneration;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Specs.Integration.TextGeneration;

[Binding]
[Scope(Tag = "getTextPromptQuery")]
public class GetTextPromptQueryStepDefinitions : TestBase
{
    private Guid _id;
    private bool _exists;
    private int _textPromptCount;
    private TextPromptDto? _response;

    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        _def = def;
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

    [Given(@"I have a expected text prompt count ""([^""]*)""")]
    public void GivenIHaveAExpectedTextPromptCount(string textPromptCount)
    {
        _textPromptCount = int.Parse(textPromptCount);
    }

    [When(@"I get a text prompt")]
    public async Task WhenIGetATextPrompt()
    {
        if (_exists)
        {
            for (int i = 0; i < _textPromptCount; i++)
            {
                var textPrompt = new TextPromptEntity()
                {
                    Id = _id,
                    Prompt = "Tell me a bedtime story",
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
                _context.TextPrompts.Add(textPrompt);
            };
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetTextPromptQuery()
        {
            Id = _id
        };

        var validator = new GetTextPromptQueryValidator();
        _validationResponse = validator.Validate(request);
        if (_validationResponse.IsValid)
            try
            {
                var handler = new GetTextPromptQueryHandler(_context, Mapper);
                _response = await handler.Handle(request, CancellationToken.None);
                _responseType = CommandResponseType.Successful;
            }
            catch (Exception e)
            {
                _responseType = HandleAssignResponseType(e);
            }
        else
            _responseType = CommandResponseType.BadRequest;
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
        if (_responseType != CommandResponseType.Successful) return;
        _response?.Id.Should().NotBeEmpty();
    }

    [Then(@"If the response is successful the response has a count matching ""([^""]*)""")]
    public void ThenIfTheResponseIsSuccessfulTheResponseHasACountMatching(string messageCount)
    {
        _response?.Responses?.Count.Should().Be(int.Parse(messageCount));
    }
}
