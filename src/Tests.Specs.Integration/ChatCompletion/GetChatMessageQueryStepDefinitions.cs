using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Specs.Integration.ChatCompletion;

[Binding]
[Scope(Tag = "getChatMessageQuery")]
public class GetChatMessageQueryStepDefinitions : TestBase
{
    private Guid _id;
    private bool _exists;
    private readonly Guid _chatSessionId = Guid.NewGuid();
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

    [Given(@"The Chat Message exists ""([^""]*)""")]
    public void GivenITheChatMessageExists(string exists)
    {
        bool.TryParse(exists, out _exists).Should().BeTrue();
    }

    [When(@"I get a Chat Message")]
    public async Task WhenIGetAChatMessage()
    {
        if (_exists)
        {
            var chatSession = ChatSessionEntity.Create(_chatSessionId, Guid.NewGuid(), "Test Session", "First Message", ChatMessageRole.assistant, "First Response");
            chatSession.Messages.Add(ChatMessageEntity.Create(_id, _chatSessionId, ChatMessageRole.user, "Test Message Content"));
            context.ChatSessions.Add(chatSession);
            await context.SaveChangesAsync(CancellationToken.None);
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
                var handler = new GetChatMessageQueryHandler(context);
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
