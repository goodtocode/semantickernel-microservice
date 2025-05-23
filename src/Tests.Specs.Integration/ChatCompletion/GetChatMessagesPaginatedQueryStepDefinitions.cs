using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Application.Common.Models;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;
using System.Security.Cryptography;

namespace Goodtocode.SemanticKernel.Specs.Integration.ChatCompletion
{
    [Binding]
    [Scope(Tag = "getChatMessagesPaginatedQuery")]
    public class GetChatMessagesPaginatedQueryStepDefinitions : TestBase
    {
        private bool _exists;
        private readonly Guid _chatSessionId = Guid.NewGuid();
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _withinDateRangeExists;
        private int _pageNumber;
        private int _pageSize;
        private PaginatedList<ChatMessageDto>? _response;

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

        [Given(@"I have a page number ""([^""]*)""")]
        public void GivenIHaveAPageNumber(string pageNumber)
        {
            int.TryParse(pageNumber, out _pageNumber).Should().BeTrue();
        }

        [Given(@"I have a page size ""([^""]*)""")]
        public void GivenIHaveAPageSize(string pageSize)
        {
            int.TryParse(pageSize, out _pageSize).Should().BeTrue(); ;
        }

        [When(@"I get the Chat Messages paginated")]
        public async Task WhenIGetTheChatMessagesPaginated()
        {
            if (_exists)
            {
                var chatSession = new ChatSessionEntity()
                {
                    Id = _chatSessionId,
                    Messages =
                     [
                         new ChatMessageEntity()
                     {
                        ChatSessionId = _chatSessionId,
                        Content = "Message",
                        Role = ChatMessageRole.user,
                        Timestamp = _startDate.AddMinutes(1)
                     }
                    ],
                    Timestamp = _startDate.AddMinutes(1),
                };
                context.ChatSessions.Add(chatSession);
                await context.SaveChangesAsync(CancellationToken.None);
            }

            var request = new GetChatMessagesPaginatedQuery()
            {
                PageNumber = _pageNumber,
                PageSize = _pageSize,
                StartDate = _startDate == default ? null : _startDate,
                EndDate = _endDate == default ? null : _endDate
            };

            var validator = new GetChatMessagesPaginatedQueryValidator();
            validationResponse = validator.Validate(request);
            if (validationResponse.IsValid)
                try
                {
                    var handler = new GetChatMessagesPaginatedQueryHandler(context, Mapper);
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
            if (responseType != CommandResponseType.Successful) return;
            _response?.TotalCount.Should().Be(_withinDateRangeExists == false ? 0 : _response.TotalCount);
        }

        [Then(@"Each Chat Message has a Key")]
        public void ThenEachChatMessageHasAKey()
        {
            if (responseType != CommandResponseType.Successful) return;
            _response?.Items.FirstOrDefault(x => x.Id == default).Should().BeNull();
        }

        [Then(@"Each Chat Message has a Date greater than start date")]
        public void ThenEachChatMessageHasADateGreaterThanStartDate()
        {
            if (responseType == CommandResponseType.Successful && _withinDateRangeExists)
                _response?.Items.FirstOrDefault(x => (_startDate == default || x.Timestamp > _startDate)).Should().NotBeNull();
        }

        [Then(@"Each Chat Message has a Date less than end date")]
        public void ThenEachChatMessageHasADateLessThanEndDate()
        {
            if (responseType == CommandResponseType.Successful && _withinDateRangeExists)
                _response?.Items.FirstOrDefault(x => (_endDate == default || x.Timestamp < _endDate)).Should().NotBeNull();
        }

        [Then(@"The response has a Page Number")]
        public void ThenTheResponseHasAPageNumber()
        {
            if (responseType != CommandResponseType.Successful) return;
            _response?.PageNumber.Should();
        }

        [Then(@"The response has a Total Pages")]
        public void ThenTheResponseHasATotalPages()
        {
            if (responseType != CommandResponseType.Successful) return;
            _response?.TotalPages.Should();
        }

        [Then(@"The response has a Total Count")]
        public void ThenTheResponseHasATotalCount()
        {
            if (responseType != CommandResponseType.Successful) return;
            _response?.TotalCount.Should();
        }
    }
}
