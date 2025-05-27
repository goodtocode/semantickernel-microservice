using Goodtocode.SemanticKernel.Core.Application.Author;
using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using System.Globalization;

namespace Goodtocode.SemanticKernel.Specs.Integration.Author;

[Binding]
[Scope(Tag = "getAuthorChatSessionQuery")]
public class GetAuthorChatSessionQueryStepDefinitions : TestBase
{
    private Guid _id;
    private Guid _chatSessionid;
    private bool _exists;
    private ChatSessionDto? _response;

    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        base.def = def;
    }

    [Given(@"I have a author id ""([^""]*)""")]
    public void GivenIHaveAAuthorId(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return;
        Guid.TryParse(id, out _id).Should().BeTrue();
    }

    [Given(@"I have a chat session id ""([^""]*)""")]
    public void GivenIHaveAChatSessionId(string chatSessionId)
    {
        if (string.IsNullOrWhiteSpace(chatSessionId)) return;
        Guid.TryParse(chatSessionId, out _chatSessionid).Should().BeTrue();
    }

    [Given(@"I the chat session exists ""([^""]*)""")]
    public void GivenITheChatSessionExists(string exists)
    {
        bool.TryParse(exists, out _exists).Should().BeTrue();
    }

    [When(@"I get a chat session")]
    public async Task WhenIGetAChatSession()
    {
        if (_exists)
        {
            var chatSession = ChatSessionEntity.Create(_id, Guid.NewGuid(), "Test Session", "First Message", ChatMessageRole.assistant, "First Response");
            context.ChatSessions.Add(chatSession);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetAuthorChatSessionQuery()
        {
            AuthorId = _id,
            ChatSessionId = _chatSessionid
        };

        var validator = new GetAuthorChatSessionQueryValidator();
        validationResponse = validator.Validate(request);
        if (validationResponse.IsValid)
            try
            {
                var handler = new GetAuthorChatSessionQueryHandler(context, Mapper);
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
}
