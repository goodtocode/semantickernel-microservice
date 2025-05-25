using Goodtocode.SemanticKernel.Core.Application.TextGeneration;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Specs.Integration.TextGeneration
{
    [Binding]
    [Scope(Tag = "deleteTextPromptCommand")]
    public class DeleteTextPromptCommandStepDefinitions : TestBase
    {
        private Guid _id;
        private bool _exists;

        [Given(@"I have a def ""([^""]*)""")]
        public void GivenIHaveADef(string def)
        {
            base.def = def;
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
                var textPrompt = TextPromptEntity.Create(_id, Guid.Empty, "Tell me a bedtime story", DateTime.UtcNow);
                textPrompt.TextResponses =
                    [
                        TextResponseEntity.Create(Guid.Empty, textPrompt.Id, "Once upon a time...")
                    ];
                context.TextPrompts.Add(textPrompt);
                await context.SaveChangesAsync(CancellationToken.None);
            }

            var validator = new DeleteTextPromptCommandValidator();
            validationResponse = await validator.ValidateAsync(request);

            if (validationResponse.IsValid)
                try
                {
                    var handler = new DeleteTextPromptCommandHandler(context);
                    await handler.Handle(request, CancellationToken.None);
                    responseType = CommandResponseType.Successful;
                }
                catch (Exception e)
                {
                    HandleAssignResponseType(e);
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
    }
}
