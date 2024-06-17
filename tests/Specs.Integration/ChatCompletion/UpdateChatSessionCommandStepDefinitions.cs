using Azure;
using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using System;
using TechTalk.SpecFlow;

namespace Goodtocode.SemanticKernel.Specs.Integration.ChatCompletion
{
    [Binding]
    [Scope(Tag = "updateChatSessionCommand")]
    public class UpdateChatSessionCommandStepDefinitions : TestBase
    {
        private bool _exists;
        private Guid _key;

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
            bool.TryParse(exists, out _exists).Should().BeTrue();
        }

        [When(@"I update the chat session")]
        public async Task WhenIUpdateTheChatSession()
        {
            var request = new PatchChatSessionCommand()
            {
                Key = _key,
                Title = "My Title"
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
