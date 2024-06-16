using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Goodtocode.SemanticKernel.Specs.Integration.ChatCompletion.Create;

[Binding]
[Scope(Tag = "createChatSessionCommand")]
public class CreateChatSessionCommandStepDefinitions : TestBase
{
    private string _message = string.Empty;
    private Guid _key;
    private bool _exists;
    private ChatSessionDto _chatSessionDto = new();

    [Given(@"I have a def ""([^""]*)""")]
    public void GivenIHaveADef(string def)
    {
        _def = def;
    }

    [Given(@"I have a initial message ""([^""]*)""")]
    public void GivenIHaveAInitialMessage(string message)
    {
        _message= message;
    }

    [Given(@"I have a chat session key ""([^""]*)""")]
    public void GivenIHaveAChatSessionKey(string key)
    {
        _key = Guid.Parse(key);
    }

    [Given(@"The chat session exists ""([^""]*)""")]
    public void GivenTheChatSessionExists(string exists)
    {
        _exists = bool.Parse(exists);
    }

    [When(@"I create a chat sesion with the message")]
    public async Task WhenICreateAChatSesionWithTheMessage()
    {
        // Setup the database if want to test existing chat session
        if (_exists)
        {
            var chatSession = new ChatSessionEntity()
            {
                Key = _key,
                Messages =
                 [
                     new ChatMessageEntity()
                 {
                     Content = _message,
                     Role = ChatMessageRole.user,
                     Timestamp = DateTime.Now
                 }
                 ],
                Timestamp = DateTime.UtcNow,
            };
            _context.ChatSessions.Add(chatSession);
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        // Test command
        var request = new CreateChatSessionCommand()
        {  
            Key = _key,
            Message = _message           
        };

        var validator = new CreateChatSessionCommandValidator();
        _validationResponse = await validator.ValidateAsync(request);

        if (_validationResponse.IsValid)
        {
            try
            {
                var chatService = new OpenAIChatCompletionService(_optionsOpenAi.ChatModelId, _optionsOpenAi.ApiKey);
                var handler = new CreateChatSessionCommandHandler(chatService, _context, Mapper);
                _chatSessionDto = await handler.Handle(request, CancellationToken.None);
                _responseType = CommandResponseType.Successful;
            }
            catch (Exception e)
            {
                HandleAssignResponseType(e);
            }
        }
        else
            _responseType = CommandResponseType.BadRequest;
    }

    [Then(@"I see the chat session created with the initial response ""([^""]*)""")]
    public void ThenISeeTheChatSessionCreatedWithTheInitialResponse(string response)
    {
        response.Should().NotBeNullOrEmpty();
    }

    [Then(@"if the response has validation issues I see the ""([^""]*)"" in the response")]
    public void ThenIfTheResponseHasValidationIssuesISeeTheInTheResponse(string expectedErrors)
    {
        HandleExpectedValidationErrorsAssertions(expectedErrors);
    }
}
