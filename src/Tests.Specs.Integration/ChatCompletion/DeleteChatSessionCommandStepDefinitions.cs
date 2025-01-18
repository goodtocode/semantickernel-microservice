using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Specs.Integration.ChatCompletion
{
    [Binding]
    [Scope(Tag = "deleteChatSessionCommand")]
    public class DeleteChatSessionCommandStepDefinitions : TestBase
    {
        private Guid _id;
        private bool _exists;

        [Given(@"I have a def ""([^""]*)""")]
        public void GivenIHaveADef(string def)
        {
            base.def = def;
        }

        [Given(@"I have a chat session id""([^""]*)""")]
        public void GivenIHaveAChatSessionKey(string id)
        {
            Guid.TryParse(id, out _id).Should().BeTrue();
        }

        [Given(@"The chat session exists ""([^""]*)""")]
        public void GivenTheChatSessionExists(string exists)
        {
            bool.TryParse(exists, out _exists).Should().BeTrue();
        }

        [When(@"I delete the chat session")]
        public async Task WhenIDeleteTheChatSession()
        {
            var request = new DeleteChatSessionCommand()
            {
                Id = _id
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

            var validator = new DeleteChatSessionCommandValidator();
            validationResponse = await validator.ValidateAsync(request);

            if (validationResponse.IsValid)
                try
                {
                    var handler = new DeleteChatSessionCommandHandler(context);
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
