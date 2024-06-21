using Goodtocode.SemanticKernel.Core.Application.Author;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Specs.Integration.Author;

[Binding]
[Scope(Tag = "getAuthorQuery")]
public class GetAuthorQueryStepDefinitions : TestBase
{
    private Guid _key;
    private bool _exists;
    private int _AuthorCount;
    private AuthorDto? _response;

    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        _def = def;
    }
    [Given(@"I have a Author key ""([^""]*)""")]
    public void GivenIHaveAAuthorKey(string AuthorKey)
    {
        if (string.IsNullOrWhiteSpace(AuthorKey)) return;
        Guid.TryParse(AuthorKey, out _key).Should().BeTrue();
    }

    [Given(@"I the Author exists ""([^""]*)""")]
    public void GivenITheAuthorExists(string exists)
    {
        bool.TryParse(exists, out _exists).Should().BeTrue();
    }


    [When(@"I get a Author")]
    public async Task WhenIGetAAuthor()
    {
        if (_exists)
        {
            var Author = new AuthorEntity()
            {
                Key = _key,
                Name = "John Doe"
            };
            _context.Authors.Add(Author);
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetAuthorQuery()
        {
            Key = _key
        };

        var validator = new GetAuthorQueryValidator();
        _validationResponse = validator.Validate(request);
        if (_validationResponse.IsValid)
            try
            {
                var handler = new GetAuthorQueryHandler(_context, Mapper);
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
        _response?.Key.Should().NotBeEmpty();
    }
}
