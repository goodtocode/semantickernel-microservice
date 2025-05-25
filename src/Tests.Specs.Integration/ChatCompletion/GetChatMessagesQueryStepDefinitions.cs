using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using System.Security.Cryptography;

namespace Goodtocode.SemanticKernel.Specs.Integration.ChatCompletion;

[Binding]
[Scope(Tag = "getChatMessagesQuery")]
public class GetChatMessagesQueryStepDefinitions : TestBase
{
    private bool _exists;
    private bool _withinDateRangeExists;
    private DateTime _endDate;
    private DateTime _startDate;
    private readonly Guid _chatSessionId = Guid.NewGuid();
    private ICollection<ChatMessageDto>? _response;

    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        base.def = def;
    }

    [Given(@"Chat Messages exist ""([^""]*)""")]
    public void GivenChatMessagesExist(string exists)
    {
        bool.TryParse(exists, out _exists).Should().BeTrue();
    }

    [Given(@"I have a start date ""([^""]*)""")]
    public void GivenIHaveAStartDate(string startDate)
    {
        if (string.IsNullOrWhiteSpace(startDate)) return;
        DateTime.TryParse(startDate, out _startDate).Should().BeTrue();
    }

    [Given(@"I have a end date ""([^""]*)""")]
    public void GivenIHaveAEndDate(string endDate)
    {
        if (string.IsNullOrWhiteSpace(endDate)) return;
        DateTime.TryParse(endDate, out _endDate).Should().BeTrue();
    }

    [Given(@"Chat Messages within the date range exists ""([^""]*)""")]
    public void GivenChatMessagesWithinTheDateRangeExists(string withinDateRangeExists)
    {
        bool.TryParse(withinDateRangeExists, out _withinDateRangeExists).Should().BeTrue();
    }

    [When(@"I get the Chat Messages")]
    public async Task WhenIGetTheChatMessages()
    {
        if (_exists)
        {
            var timestamp = _withinDateRangeExists ? _startDate.AddMinutes(1) : _startDate.AddMinutes(-1);
            var chatSession = ChatSessionEntity.Create(_chatSessionId, Guid.NewGuid(), "Test Session", "First Message", "First Response", timestamp);
            context.ChatSessions.Add(chatSession);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetChatMessagesQuery()
        {
            StartDate = _startDate == default ? null : _startDate,
            EndDate = _endDate == default ? null : _endDate
        };

        var validator = new GetChatMessagesQueryValidator();
        validationResponse = validator.Validate(request);
        if (validationResponse.IsValid)
            try
            {
                var handler = new GetChatMessagesQueryHandler(context, Mapper);
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

    [Then(@"The response has a collection of Chat Messages")]
    public void ThenTheResponseHasACollectionOfChatMessages()
    {
        _response?.Count.Should().Be(_withinDateRangeExists == false ? 0 : _response.Count);
    }

    [Then(@"Each Chat Message has a Key")]
    public void ThenEachChatMessageHasAKey()
    {
        _response?.FirstOrDefault(x => x.Id == default).Should().BeNull();
    }

    [Then(@"Each Chat Message has a Date greater than start date")]
    public void ThenEachChatMessageHasADateGreaterThanStartDate()
    {
        if (_withinDateRangeExists)
            _response?.FirstOrDefault(x => (_startDate == default || x.Timestamp > _startDate)).Should().NotBeNull();
    }

    [Then(@"Each Chat Message has a Date less than end date")]
    public void ThenEachChatMessageHasADateLessThanEndDate()
    {
        if (_withinDateRangeExists)
            _response?.FirstOrDefault(x => (_endDate == default || x.Timestamp < _endDate)).Should().NotBeNull();
    }
}
