using Goodtocode.SemanticKernel.Core.Application.Audio;
using Goodtocode.SemanticKernel.Core.Domain.Audio;

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
        base.def = def;
    }

    [Given(@"I have a text audio id ""([^""]*)""")]
    public void GivenIHaveATextAudioKey(string textPromptKey)
    {
        if (string.IsNullOrWhiteSpace(textPromptKey)) return;
        Guid.TryParse(textPromptKey, out _id).ShouldBeTrue();
    }

    [Given(@"I the text audio exists ""([^""]*)""")]
    public void GivenIThetextAudioExists(string exists)
    {
        bool.TryParse(exists, out _exists).ShouldBeTrue();
    }

    [When(@"I get a text audio")]
    public async Task WhenIGetATextAudio()
    {
        if (_exists)
        {
            var textAudio = TextAudioEntity.Create(_id, Guid.NewGuid(),
                "Audio of a simple geometric design consisting of two yellow squares and one blue square. " +
                    "The blue square is placed at a 45-degree angle, positioned centrally below the two yellow squares, creating a symmetrical arrangement. " +
                    "Each square is connected by what appears to be black lines or sticks, suggesting they may represent nodes or elements in a network or structure. " +
                    "The background is white, which contrasts with the bright colors of the squares.",
                new ReadOnlyMemory<byte>([0x01, 0x02, 0x03, 0x04]));
            context.TextAudio.Add(textAudio);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetTextAudioQuery()
        {
            Id = _id
        };

        var validator = new GetTextAudioQueryValidator();
        validationResponse = validator.Validate(request);
        if (validationResponse.IsValid)
            try
            {
                var handler = new GetTextAudioQueryHandler(context);
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
        _response?.Id.ShouldNotBeEmpty();
    }
}
