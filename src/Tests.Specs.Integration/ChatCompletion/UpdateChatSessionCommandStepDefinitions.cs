using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Specs.Integration.ChatCompletion
{
    [Binding]
    [Scope(Tag = "updateChatSessionCommand")]
    public class UpdateChatSessionCommandStepDefinitions : TestBase
    {
        private bool _exists;
        private Guid _id;

        [Given(@"I have a def ""([^""]*)""")]
        public void GivenIHaveADef(string def)
        {
            base.def = def;
        }

        [Given(@"I have a chat session id ""([^""]*)""")]
        public void GivenIHaveAChatSessionId(string id)
        {
            Guid.TryParse(id, out _id).Should().BeTrue();
        }

        [Given(@"the chat session exists ""([^""]*)""")]
        public void GivenTheChatSessionExists(string exists)
        {
            bool.TryParse(exists, out _exists).Should().BeTrue();
        }

        [When(@"I update the chat session")]
        public async Task WhenIUpdateTheChatSession()
        {
            var request = new UpdateChatSessionCommand()
            {
                Id = _id,
                Title = "My Title"
            };

            if (_exists)
            {
                var chatSession = new ChatSessionEntity()
                {
                    Id = _id,
                    Title = "Initial Title",
                    Messages = [
                        new ChatMessageEntity()
                        {
                            Content = "Initial Content",
                            Role = ChatMessageRole.user,
                            Timestamp = DateTime.Now
                        }
                    ],
                    Timestamp = DateTime.UtcNow,
                };
                context.ChatSessions.Add(chatSession);
                await context.SaveChangesAsync(CancellationToken.None);
            }

            var validator = new UpdateChatSessionCommandValidator();
            validationResponse = await validator.ValidateAsync(request);

            if (validationResponse.IsValid)
                try
                {
                    var handler = new UpdateChatSessionCommandHandler(context);
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
