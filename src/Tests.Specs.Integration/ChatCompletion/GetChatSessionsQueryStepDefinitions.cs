using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Specs.Integration.ChatCompletion;

[Binding]
[Scope(Tag = "getChatSessionsQuery")]
public class GetChatSessionsQueryStepDefinitions : TestBase
{
    private bool _exists;
    private bool _withinDateRangeExists;
    private DateTime _endDate;
    private DateTime _startDate;
    private ICollection<ChatSessionDto>? _response;

    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        base.def = def;
    }

    [Given(@"Chat Sessions exist ""([^""]*)""")]
    public void GivenChatSessionsExist(string exists)
    {
        bool.TryParse(exists, out _exists).Should().BeTrue();
    }

    [Given(@"chat sessions within the date range exists ""([^""]*)""")]
    public void GivenChatSessionsWithinTheDateRangeExists(string withinDateRangeExists)
    {
        bool.TryParse(withinDateRangeExists, out _withinDateRangeExists).Should().BeTrue();
    }

    [Given(@"I have a start date ""([^""]*)""")]
    public void GivenIHaveAStartDate(string startDate)
    {
        if (string.IsNullOrWhiteSpace(startDate)) return;
        DateTime.TryParse(startDate, out _startDate).Should().BeTrue();
        _startDate = DateTime.UtcNow.AddMinutes(_withinDateRangeExists ? -1 : 1); //Handle for desired not-found scenarios
    }

    [Given(@"I have a end date ""([^""]*)""")]
    public void GivenIHaveAEndDate(string endDate)
    {
        if (string.IsNullOrWhiteSpace(endDate)) return;
        DateTime.TryParse(endDate, out _endDate).Should().BeTrue();
    }

    [When(@"I get the chat sessions")]
    public async Task WhenIGetTheChatSessions()
    {
        if (_exists)
        {
            var chatSession = ChatSessionEntity.Create(Guid.NewGuid(), Guid.NewGuid(), "Test Session", "First Message", ChatMessageRole.assistant, "First Response");
            context.ChatSessions.Add(chatSession);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetChatSessionsQuery()
        {
            StartDate = _startDate == default ? null : _startDate,
            EndDate = _endDate == default ? null : _endDate
        };

        var validator = new GetChatSessionsQueryValidator();
        validationResponse = validator.Validate(request);
        if (validationResponse.IsValid)
            try
            {
                var handler = new GetChatSessionsQueryHandler(context);
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

    [Then(@"The response has a collection of chat sessions")]
    public void ThenTheResponseHasACollectionOfChatSessions()
    {
        _response?.Count.Should().Be(_withinDateRangeExists == false ? 0 : _response.Count);
    }

    [Then(@"Each chat session has a Key")]
    public void ThenEachChatSessionHasAKey()
    {
        _response?.FirstOrDefault(x => x.Id == default).Should().BeNull();
    }

    [Then(@"Each chat session has a Date greater than start date")]
    public void ThenEachChatSessionHasADateGreaterThanStartDate()
    {
        if (_withinDateRangeExists)
            _response?.FirstOrDefault(x => (_startDate == default || x.Timestamp > _startDate)).Should().NotBeNull();
    }

    [Then(@"Each chat session has a Date less than end date")]
    public void ThenEachChatSessionHasADateLessThanEndDate()
    {
        if (_withinDateRangeExists)
            _response?.FirstOrDefault(x => (_endDate == default || x.Timestamp < _endDate)).Should().NotBeNull();
    }
}
