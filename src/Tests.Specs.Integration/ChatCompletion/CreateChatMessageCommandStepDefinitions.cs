using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Goodtocode.SemanticKernel.Specs.Integration.ChatCompletion;

[Binding]
[Scope(Tag = "createChatMessageCommand")]
public class CreateChatMessageCommandStepDefinitions : TestBase
{
    private string _message = string.Empty;
    private Guid _id;
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

    [Given(@"I have a Chat Message id ""([^""]*)""")]
    public void GivenIHaveAChatMessageKey(string id)
    {
        _id = Guid.Parse(id);
    }

    [Given(@"The Chat Message exists ""([^""]*)""")]
    public void GivenTheChatMessageExists(string exists)
    {
        _exists = bool.Parse(exists);
    }

    [When(@"I create a Chat Message with the message")]
    public async Task WhenICreateAChatMessageWithTheMessage()
    {
        // Setup the database if want to test existing records
        if (_exists)
        {
            var ChatMessage = new ChatMessageEntity()
            {
                Id = _id,
                Content = _message,
                Role = ChatMessageRole.user,
                Timestamp = DateTime.Now
            };
            context.ChatMessages.Add(ChatMessage);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        // Test command
        var request = new CreateChatMessageCommand()
        {
            Id = _id,
            Message = _message
        };

        var validator = new CreateChatMessageCommandValidator();
        validationResponse = await validator.ValidateAsync(request);

        if (validationResponse.IsValid)
        {
            try
            {
                var chatService = new OpenAIChatCompletionService(optionsOpenAi.ChatCompletionModelId, optionsOpenAi.ApiKey);
                var handler = new CreateChatMessageCommandHandler(chatService, context, Mapper);
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

    [Then(@"I see the Chat Message created with the initial response ""([^""]*)""")]
    public void ThenISeeTheChatMessageCreatedWithTheInitialResponse(string response)
    {
        HandleHasResponseType(response);
    }

    [Then(@"if the response has validation issues I see the ""([^""]*)"" in the response")]
    public void ThenIfTheResponseHasValidationIssuesISeeTheInTheResponse(string expectedErrors)
    {
        HandleExpectedValidationErrorsAssertions(expectedErrors);
    }
}
