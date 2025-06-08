using Goodtocode.SemanticKernel.Core.Application.Image;
using Goodtocode.SemanticKernel.Core.Domain.Image;

namespace Goodtocode.SemanticKernel.Specs.Integration.Image;

[Binding]
[Scope(Tag = "getTextImageQuery")]
public class GetTextImageQueryStepDefinitions : TestBase
{
    private Guid _id;
    private bool _exists;
    private TextImageDto? _response;

    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        base.def = def;
    }

    [Given(@"I have a text image id ""([^""]*)""")]
    public void GivenIHaveATextImageKey(string textPromptKey)
    {
        if (string.IsNullOrWhiteSpace(textPromptKey)) return;
        Guid.TryParse(textPromptKey, out _id).Should().BeTrue();
    }

    [Given(@"I the text image exists ""([^""]*)""")]
    public void GivenIThetextImageExists(string exists)
    {
        bool.TryParse(exists, out _exists).Should().BeTrue();
    }

    [When(@"I get a text image")]
    public async Task WhenIGetATextImage()
    {
        if (_exists)
        {
            var textImage = TextImageEntity.Create(_id, "Image of a simple geometric design consisting of two yellow squares and one blue square. " +
                    "The blue square is placed at a 45-degree angle, positioned centrally below the two yellow squares, creating a symmetrical arrangement. " +
                    "Each square is connected by what appears to be black lines or sticks, suggesting they may represent nodes or elements in a network or structure. " +
                    "The background is white, which contrasts with the bright colors of the squares.",
                1024,
                1024,
                new ReadOnlyMemory<byte>([0x01, 0x02, 0x03, 0x04]));
            context.TextImages.Add(textImage);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetTextImageQuery()
        {
            Id = _id
        };

        var validator = new GetTextImageQueryValidator();
        validationResponse = validator.Validate(request);
        if (validationResponse.IsValid)
            try
            {
                var handler = new GetTextImageQueryHandler(context, Mapper);
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
