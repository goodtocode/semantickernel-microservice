using Goodtocode.SemanticKernel.Core.Application.Author;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Specs.Integration.Author;

[Binding]
[Scope(Tag = "createAuthorCommand")]
public class CreateAuthorCommandStepDefinitions : TestBase
{
    private string _name = string.Empty;
    private Guid _id;
    private bool _exists;

    [Given(@"I have a def ""([^""]*)""")]
    public void GivenIHaveADef(string def)
    {
        _def = def;
    }

    [Given(@"I have a name ""([^""]*)""")]
    public void GivenIHaveAName(string name)
    {
        _name = name;
    }

    [Given(@"I have a Author id ""([^""]*)""")]
    public void GivenIHaveAAuthorId(string id)
    {
        _id = Guid.Parse(id);
    }

    [Given(@"The Author exists ""([^""]*)""")]
    public void GivenTheAuthorExists(string exists)
    {
        bool.TryParse(exists, out _exists).Should().BeTrue();
    }

    [When(@"I create a author")]
    public async Task WhenICreateAAuthor()
    {
        // Setup the database if want to test existing records
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

        // Test command
        var request = new CreateAuthorCommand()
        {
            Id = _id,
            Name = _name
        };

        var validator = new CreateAuthorCommandValidator();
        _validationResponse = await validator.ValidateAsync(request);

        if (_validationResponse.IsValid)
        {
            try
            {
                var handler = new CreateAuthorCommandHandler( _context, Mapper);
                await handler.Handle(request, CancellationToken.None);
                _responseType = CommandResponseType.Successful;
            }
            catch (Exception e)
            {
                HandleAssignResponseType(e);
            }
        }
        else
            _responseType = CommandResponseType.BadRequest;
    }


    [Then(@"I see the Author created with the initial response ""([^""]*)""")]
    public void ThenISeeTheAuthorCreatedWithTheInitialResponse(string response)
    {
        HandleHasResponseType(response);
    }

    [Then(@"if the response has validation issues I see the ""([^""]*)"" in the response")]
    public void ThenIfTheResponseHasValidationIssuesISeeTheInTheResponse(string expectedErrors)
    {
        HandleExpectedValidationErrorsAssertions(expectedErrors);
    }

}