using Goodtocode.SemanticKernel.Core.Application.Image;
using Goodtocode.SemanticKernel.Core.Domain.Image;
using System.Text;

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
        _def = def;
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
            var textImage = new TextImageEntity()
            {
                Id = _id,
                Description = "Image of a simple geometric design consisting of two yellow squares and one blue square. " +
                    "The blue square is placed at a 45-degree angle, positioned centrally below the two yellow squares, creating a symmetrical arrangement. " +
                    "Each square is connected by what appears to be black lines or sticks, suggesting they may represent nodes or elements in a network or structure. " +
                    "The background is white, which contrasts with the bright colors of the squares.",
                Width = 1024,
                Height = 1024,
                ImageBytes = Encoding.UTF8.GetString(new byte[16 * 16 * 4]),
                Timestamp = DateTime.UtcNow
            };
            _context.TextImages.Add(textImage);
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetTextImageQuery()
        {
            Id = _id
        };

        var validator = new GetTextImageQueryValidator();
        _validationResponse = validator.Validate(request);
        if (_validationResponse.IsValid)
            try
            {
                var handler = new GetTextImageQueryHandler(_context, Mapper);
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
