using Goodtocode.SemanticKernel.Core.Application.TextGeneration;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Specs.Integration.TextGeneration
{
    [Binding]
    public class DeleteTextPromptCommandStepDefinitions : TestBase
    {
        private Guid _id;
        private bool _exists;

        [Given(@"I have a def ""([^""]*)""")]
        public void GivenIHaveADef(string def)
        {
            _def = def;
        }

        [Given(@"I have a text prompt id""([^""]*)""")]
        public void GivenIHaveATextPromptKey(string id)
        {
            Guid.TryParse(id, out _id).Should().BeTrue();
        }

        [Given(@"The text prompt exists ""([^""]*)""")]
        public void GivenTheTextPromptExists(string exists)
        {
            bool.TryParse(exists, out _exists).Should().BeTrue();
        }

        [When(@"I delete the text prompt")]
        public async Task WhenIDeleteTheTextPrompt()
        {
            var request = new DeleteTextPromptCommand()
            {
                Id = _id
            };

            if (_exists)
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
                await _context.SaveChangesAsync(CancellationToken.None);
            }

            var validator = new DeleteTextPromptCommandValidator();
            _validationResponse = await validator.ValidateAsync(request);

            if (_validationResponse.IsValid)
                try
                {
                    var handler = new DeleteTextPromptCommandHandler(_context);
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
