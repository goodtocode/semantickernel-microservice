using Goodtocode.SemanticKernel.Core.Application.Author;
using Goodtocode.SemanticKernel.Core.Domain.Author;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Specs.Integration.Author
{
    [Binding]
    [Scope(Tag = "updateAuthorCommand")]
    public class UpdateAuthorCommandStepDefinitions : TestBase
    {
        private bool _exists;
        private Guid _key;

        [Given(@"I have a def ""([^""]*)""")]
        public void GivenIHaveADef(string def)
        {
            _def = def;
        }

        [Given(@"I have a Author key ""([^""]*)""")]
        public void GivenIHaveAAuthorKey(string key)
        {
            Guid.TryParse(key, out _key).Should().BeTrue();
        }

        [Given(@"the Author exists ""([^""]*)""")]
        public void GivenTheAuthorExists(string exists)
        {
            bool.TryParse(exists, out _exists).Should().BeTrue();
        }

        [When(@"I update the Author")]
        public async Task WhenIUpdateTheAuthor()
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

            var request = new UpdateAuthorCommand()
            {
                Key = _key,
                Name = "Joe Doe"
            };

            var validator = new UpdateAuthorCommandValidator();
            _validationResponse = await validator.ValidateAsync(request);

            if (_validationResponse.IsValid)
                try
                {
                    var handler = new UpdateAuthorCommandHandler(_context);
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
