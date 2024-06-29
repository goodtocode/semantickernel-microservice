using Goodtocode.SemanticKernel.Core.Application.Author;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Specs.Integration.Author;

[Binding]
[Scope(Tag = "getAuthorQuery")]
public class GetAuthorQueryStepDefinitions : TestBase
{
    private Guid _id;
    private bool _exists;
    private AuthorDto? _response;

    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        _def = def;
    }
    [Given(@"I have a Author id ""([^""]*)""")]
    public void GivenIHaveAAuthorKey(string authorId)
    {
        if (string.IsNullOrWhiteSpace(authorId)) return;
        Guid.TryParse(authorId, out _id).Should().BeTrue();
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
                Id = _id,
                Name = "John Doe"
            };
            _context.Authors.Add(Author);
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetAuthorQuery()
        {
            AuthorId = _id
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

    [Then(@"If the response is successful the response has a Id")]
    public void ThenIfTheResponseIsSuccessfulTheResponseHasAKey()
    {
        if (_responseType != CommandResponseType.Successful) return;
        _response?.Id.Should().NotBeEmpty();
    }
}
