using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using System.Globalization;

namespace Goodtocode.SemanticKernel.Specs.Integration.ChatCompletion;

[Binding]
[Scope(Tag = "getChatMessageQuery")]
public class GetChatMessageQueryStepDefinitions : TestBase
{
    private Guid _id;
    private bool _exists;
    private int _ChatMessageCount;
    private ChatMessageDto? _response;

    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        base.def = def;
    }

    [Given(@"I have a Chat Message id ""([^""]*)""")]
    public void GivenIHaveAChatMessageId(string ChatMessageKey)
    {
        if (string.IsNullOrWhiteSpace(ChatMessageKey)) return;
        Guid.TryParse(ChatMessageKey, out _id).Should().BeTrue();
    }

    [Given(@"I the Chat Message exists ""([^""]*)""")]
    public void GivenITheChatMessageExists(string exists)
    {
        bool.TryParse(exists, out _exists).Should().BeTrue();
    }

    [Given(@"I have a expected Chat Message count ""([^""]*)""")]
    public void GivenIHaveAExpectedChatMessageCount(string ChatMessageCount)
    {
        _ChatMessageCount = int.Parse(ChatMessageCount, CultureInfo.InvariantCulture);
    }

    [When(@"I get a Chat Message")]
    public async Task WhenIGetAChatMessage()
    {
        if (_exists)
        {
            // Setup Session
            var chatSession = new ChatSessionEntity()
            {
                Id = _id,
                Messages =
                 [
                     new ChatMessageEntity()
                     {
                         Content = "Test Message",
                         Role = ChatMessageRole.user,
                         Timestamp = DateTime.Now
                     }
                 ],
                Timestamp = DateTime.UtcNow,
            };
            context.ChatSessions.Add(chatSession);
            await context.SaveChangesAsync(CancellationToken.None);

            // Setup Messages
            var messages = new List<ChatMessageEntity>();
            for (int i = 0; i < _ChatMessageCount; i++)
            {
                var ChatMessage = new ChatMessageEntity()
                {
                    Id = _id,
                    ChatSessionId = chatSession.Id,
                    Content = "Test Message",
                    Role = ChatMessageRole.user,
                    Timestamp = DateTime.UtcNow,
                };
                context.ChatMessages.Add(ChatMessage);
                await context.SaveChangesAsync(CancellationToken.None);
            };
        }

        var request = new GetChatMessageQuery()
        {
            Id = _id
        };

        var validator = new GetChatMessageQueryValidator();
        validationResponse = validator.Validate(request);
        if (validationResponse.IsValid)
            try
            {
                var handler = new GetChatMessageQueryHandler(context, Mapper);
                _response = await handler.Handle(request, CancellationToken.None);
                responseType = CommandResponseType.Successful;
            }
            catch (Exception e)
            {
                responseType = HandleAssignResponseType(e);
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

    [Then(@"If the response is successful the response has a Id")]
    public void ThenIfTheResponseIsSuccessfulTheResponseHasAId()
    {
        if (responseType != CommandResponseType.Successful) return;
        _response?.Id.Should().NotBeEmpty();
    }

    [Then(@"If the response is successful the response has a count matching ""([^""]*)""")]
    public void ThenIfTheResponseIsSuccessfulTheResponseHasACountMatching(string messageContent)
    {
       _response?.Content?.Should().Be(messageContent);
    }
}
