using Goodtocode.SemanticKernel.Core.Application.Audio;
using Goodtocode.SemanticKernel.Core.Domain.Audio;
using System.Text;

namespace Goodtocode.SemanticKernel.Specs.Integration.Audio;

[Binding]
[Scope(Tag = "getTextAudioQuery")]
public class GetTextAudioQueryStepDefinitions : TestBase
{
    private Guid _id;
    private bool _exists;
    private TextAudioDto? _response;

    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        _def = def;
    }

    [Given(@"I have a text audio id ""([^""]*)""")]
    public void GivenIHaveATextAudioKey(string textPromptKey)
    {
        if (string.IsNullOrWhiteSpace(textPromptKey)) return;
        Guid.TryParse(textPromptKey, out _id).Should().BeTrue();
    }

    [Given(@"I the text audio exists ""([^""]*)""")]
    public void GivenIThetextAudioExists(string exists)
    {
        bool.TryParse(exists, out _exists).Should().BeTrue();
    }

    [When(@"I get a text audio")]
    public async Task WhenIGetATextAudio()
    {
        if (_exists)
        {
            var textAudio = new TextAudioEntity()
            {
                Id = _id,
                Description = "Audio of a simple geometric design consisting of two yellow squares and one blue square. " +
                    "The blue square is placed at a 45-degree angle, positioned centrally below the two yellow squares, creating a symmetrical arrangement. " +
                    "Each square is connected by what appears to be black lines or sticks, suggesting they may represent nodes or elements in a network or structure. " +
                    "The background is white, which contrasts with the bright colors of the squares.",
                AudioBytes = [0x01, 0x02, 0x03, 0x04],
                Timestamp = DateTime.UtcNow
            };
            _context.TextAudio.Add(textAudio);
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetTextAudioQuery()
        {
            Id = _id
        };

        var validator = new GetTextAudioQueryValidator();
        _validationResponse = validator.Validate(request);
        if (_validationResponse.IsValid)
            try
            {
                var handler = new GetTextAudioQueryHandler(_context, Mapper);
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
}
