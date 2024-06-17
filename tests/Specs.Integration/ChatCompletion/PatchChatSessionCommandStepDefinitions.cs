using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Specs.Integration.ChatCompletion
{
    [Binding]
    [Scope(Tag = "patchChatSessionCommand")]
    public class PatchChatSessionCommandStepDefinitions : TestBase
    {
        private Guid _key;
        private bool _exists;
        private string _title = string.Empty;

        [Given(@"I have a def ""([^""]*)""")]
        public void GivenIHaveADef(string def)
        {
            _def = def;
        }

        [Given(@"I have a chat session key ""([^""]*)""")]
        public void GivenIHaveAChatSessionKey(string key)
        {
            Guid.TryParse(key, out _key);
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
                Key = _key,
                Title = _title
            };

            if (_exists)
            {
                var chatSession = new ChatSessionEntity()
                {
                    Key = _key,
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
                _context.ChatSessions.Add(chatSession);
                await _context.SaveChangesAsync(CancellationToken.None);
            }

            var validator = new PatchChatSessionCommandValidator();
            _validationResponse = await validator.ValidateAsync(request);

            if (_validationResponse.IsValid)
                try
                {
                    var handler = new PatchChatSessionCommandHandler(_context);
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
