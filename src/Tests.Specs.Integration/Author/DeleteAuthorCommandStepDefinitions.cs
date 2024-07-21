using Goodtocode.SemanticKernel.Core.Application.Author;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Specs.Integration.Author
{
    [Binding]
    [Scope(Tag = "deleteAuthorCommand")]
    public class DeleteAuthorCommandStepDefinitions : TestBase
    {
        private bool _exists;
        private Guid _id;

        [Given(@"I have a def ""([^""]*)""")]
        public void GivenIHaveADef(string def)
        {
            _def = def;
        }

        [Given(@"I have a author id""([^""]*)""")]
        public void GivenIHaveAAuthorId(string id)
        {
            Guid.TryParse(id, out _id).Should().BeTrue();
        }

        [Given(@"The author exists ""([^""]*)""")]
        public void GivenTheAuthorExists(string exists)
        {
            bool.TryParse(exists, out _exists).Should().BeTrue();
        }

        [When(@"I delete the author")]
        public async Task WhenIDeleteTheAuthor()
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

            var request = new DeleteAuthorCommand()
            {
                Id = _id
            };

            var validator = new DeleteAuthorCommandValidator();
            _validationResponse = await validator.ValidateAsync(request);

            if (_validationResponse.IsValid)
                try
                {
                    var handler = new DeleteAuthorCommandHandler(_context);
                    await handler.Handle(request, CancellationToken.None);
                    _responseType = CommandResponseType.Successful;
                }
                catch (Exception e)
                {
                    HandleAssignResponseType(e);
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
    }
}
