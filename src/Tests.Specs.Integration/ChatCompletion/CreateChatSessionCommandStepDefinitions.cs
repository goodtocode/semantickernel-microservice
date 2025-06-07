using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Specs.Integration.ChatCompletion;

[Binding]
[Scope(Tag = "createChatSessionCommand")]
public class CreateChatSessionCommandStepDefinitions : TestBase
{
    private string _message = string.Empty;
    private Guid _id;
    private readonly Guid _authorId = Guid.NewGuid();
    private bool _exists;

    [Given(@"I have a def ""([^""]*)""")]
    public void GivenIHaveADef(string def)
    {
        base.def = def;
    }

    [Given(@"I have a initial message ""([^""]*)""")]
    public void GivenIHaveAInitialMessage(string message)
    {
        _message = message;
    }

    [Given(@"I have a chat session id ""([^""]*)""")]
    public void GivenIHaveAChatSessionKey(string id)
    {
        _id = Guid.Parse(id);
    }

    [Given(@"The chat session exists ""([^""]*)""")]
    public void GivenTheChatSessionExists(string exists)
    {
        _exists = bool.Parse(exists);
    }

    [When(@"I create a chat session with the message")]
    public async Task WhenICreateAChatSessionWithTheMessage()
    {
        // Setup the database if want to test existing records
        if (_exists)
        {
            var chatSession = ChatSessionEntity.Create(_id, _authorId, "Test Session", _message, ChatMessageRole.assistant, "First Response");
            context.ChatSessions.Add(chatSession);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // Test command
        var request = new CreateChatSessionCommand()
        {
            Id = _id,
            Title = def,
            AuthorId = _authorId,
            Message = _message
        };

        var validator = new CreateChatSessionCommandValidator();
        validationResponse = await validator.ValidateAsync(request);

        if (validationResponse.IsValid)
        {
            try
            {                
                var handler = new CreateChatSessionCommandHandler(kernel, context, Mapper);
                await handler.Handle(request, CancellationToken.None);
                responseType = CommandResponseType.Successful;
            }
            catch (Exception e)
            {
                HandleAssignResponseType(e);
            }
        }
        else
            responseType = CommandResponseType.BadRequest;
    }

    [Then(@"I see the chat session created with the initial response ""([^""]*)""")]
    public void ThenISeeTheChatSessionCreatedWithTheInitialResponse(string response)
    {
        HandleHasResponseType(response);
    }

    [Then(@"if the response has validation issues I see the ""([^""]*)"" in the response")]
    public void ThenIfTheResponseHasValidationIssuesISeeTheInTheResponse(string expectedErrors)
    {
        HandleExpectedValidationErrorsAssertions(expectedErrors);
    }
}
