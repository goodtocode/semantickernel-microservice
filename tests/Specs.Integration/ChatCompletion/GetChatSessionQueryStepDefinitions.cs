using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Specs.Integration.ChatCompletion;

[Binding]
[Scope(Tag = "getChatSessionQuery")]
public class GetChatSessionQueryStepDefinitions : TestBase
{
    private Guid _id;
    private bool _exists;
    private int _chatSessionCount;
    private ChatSessionDto? _response;

    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        _def = def;
    }

    [Given(@"I have a chat session id ""([^""]*)""")]
    public void GivenIHaveAChatSessionId(string chatSessionKey)
    {
        if (string.IsNullOrWhiteSpace(chatSessionKey)) return;
        Guid.TryParse(chatSessionKey, out _id).Should().BeTrue();
    }

    [Given(@"I the chat session exists ""([^""]*)""")]
    public void GivenITheChatSessionExists(string exists)
    {
        bool.TryParse(exists, out _exists).Should().BeTrue();
    }

    [Given(@"I have a expected chat session count ""([^""]*)""")]
    public void GivenIHaveAExpectedChatSessionCount(string chatSessionCount)
    {
        _chatSessionCount = int.Parse(chatSessionCount);
    }

    [When(@"I get a chat session")]
    public async Task WhenIGetAChatSession()
    {
        if (_exists)
        {
            var messages = new List<ChatMessageEntity>();
            for (int i = 0; i < _chatSessionCount; i++)
            {
                messages.Add(new ChatMessageEntity()
                {
                    Content = "Test Message",
                    Role = ChatMessageRole.user,
                    Timestamp = DateTime.Now
                });
            };
            var chatSession = new ChatSessionEntity()
            {
                Id = _id,
                Messages = messages,
                Timestamp = DateTime.UtcNow,
            };
            _context.ChatSessions.Add(chatSession);
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetChatSessionQuery()
        {
            Id = _id
        };

        var validator = new GetChatSessionQueryValidator();
        _validationResponse = validator.Validate(request);
        if (_validationResponse.IsValid)
            try
            {
                var handler = new GetChatSessionQueryHandler(_context, Mapper);
                _response = await handler.Handle(request, CancellationToken.None);
                _responseType = CommandResponseType.Successful;
            }
            catch (Exception e)
            {
                _responseType = HandleAssignResponseType(e);
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

    [Then(@"If the response is successful the response has a Id")]
    public void ThenIfTheResponseIsSuccessfulTheResponseHasAId()
    {
        if (_responseType != CommandResponseType.Successful) return;
        _response?.Id.Should().NotBeEmpty();
    }

    [Then(@"If the response is successful the response has a count matching ""([^""]*)""")]
    public void ThenIfTheResponseIsSuccessfulTheResponseHasACountMatching(string messageCount)
    {
        _response?.Messages?.Count.Should().Be(int.Parse(messageCount));
    }
}
