using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Specs.Integration.ChatCompletion
{
    [Binding]
    [Scope(Tag = "patchChatSessionCommand")]
    public class PatchChatSessionCommandStepDefinitions : TestBase
    {
        private Guid _id;
        private bool _exists;
        private string _title = string.Empty;

        [Given(@"I have a def ""([^""]*)""")]
        public void GivenIHaveADef(string def)
        {
            base.def = def;
        }

        [Given(@"I have a chat session id ""([^""]*)""")]
        public void GivenIHaveAChatSessionId(string id)
        {
            Guid.TryParse(id, out _id).ShouldBeTrue();
        }

        [Given(@"the chat session exists ""([^""]*)""")]
        public void GivenTheChatSessionExists(string exists)
        {
            _exists = bool.Parse(exists);
        }

        [Given(@"I have a new chat session title ""([^""]*)""")]
        public void GivenIHaveANewChatSessionTitle(string title)
        {
            _title = title;
        }

        [When(@"I patch the chatSession")]
        public async Task WhenIPatchTheChatSession()
        {
            var request = new PatchChatSessionCommand()
            {
                Id = _id,
                Title = _title
            };

            if (_exists)
            {
                var chatSession = ChatSessionEntity.Create(_id, Guid.NewGuid(), "Test Session", ChatMessageRole.assistant, "First Message", "First Response");
                context.ChatSessions.Add(chatSession);
                await context.SaveChangesAsync(CancellationToken.None);
            }

            var validator = new PatchChatSessionCommandValidator();
            validationResponse = await validator.ValidateAsync(request);

            if (validationResponse.IsValid)
                try
                {
                    var handler = new PatchChatSessionCommandHandler(context);
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
