using Goodtocode.SemanticKernel.Core.Application.Author;
using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Application.Common.Models;
using Goodtocode.SemanticKernel.Core.Domain.Author;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Specs.Integration.Author
{
    [Binding]
    [Scope(Tag = "getAuthorChatSessionsPaginatedQuery")]
    public class GetAuthorChatSessionsPaginatedQueryStepDefinitions : TestBase
    {
        private bool _exists;
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _withinDateRangeExists;
        private int _pageNumber;
        private int _pageSize;
        private PaginatedList<ChatSessionDto>? _response;
        private Guid _id;

        [Given(@"I have a definition ""([^""]*)""")]
        public void GivenIHaveADefinition(string def)
        {
            base.def = def;
        }

        [Given(@"Chat Sessions exist ""([^""]*)""")]
        public void GivenAuthorChatSessionsExist(string exists)
        {
            bool.TryParse(exists, out _exists).Should().BeTrue();
        }

        [Given(@"I have a Author id ""([^""]*)""")]
        public void GivenIHaveAAuthorId(string authorId)
        {
            if (string.IsNullOrWhiteSpace(authorId)) return;
            Guid.TryParse(authorId, out _id).Should().BeTrue();
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

        [Given(@"chat sessions within the date range exists ""([^""]*)""")]
        public void GivenAuthorChatSessionsWithinTheDateRangeExists(string withinDateRangeExists)
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

        [When(@"I get the chat sessions paginated")]
        public async Task WhenIGetTheAuthorChatSessionsPaginated()
        {
            if (_exists)
            {
                var author = AuthorEntity.Create(_id, "John Doe");
                context.Authors.Add(author);
                await context.SaveChangesAsync(CancellationToken.None);
                var chatSession = ChatSessionEntity.Create(_id, _id, "Test Session", ChatMessageRole.assistant, "First Message", "First Response");
                for(var count = 0; count <= _pageSize; count++)
                {                    
                    context.ChatMessages.Add(ChatMessageEntity.Create(Guid.NewGuid(), _id, ChatMessageRole.user, $"Message {count + 1}"));
                    chatSession.Messages.Add(ChatMessageEntity.Create(Guid.NewGuid(), _id, ChatMessageRole.user, $"Message {count + 1}"));
                    context.ChatMessages.Add(ChatMessageEntity.Create(Guid.NewGuid(), _id, ChatMessageRole.assistant, $"Response {count + 1}"));
                    chatSession.Messages.Add(ChatMessageEntity.Create(Guid.NewGuid(), _id, ChatMessageRole.assistant, $"Response {count + 1}"));
                }
                context.ChatSessions.Add(chatSession);
                await context.SaveChangesAsync(CancellationToken.None);
            }

            var request = new GetAuthorChatSessionsPaginatedQuery()
            {
                AuthorId = _id,
                PageNumber = _pageNumber,
                PageSize = _pageSize,
                StartDate = _startDate == default ? null : _startDate,
                EndDate = _endDate == default ? null : _endDate
            };

            var validator = new GetAuthorChatSessionsPaginatedQueryValidator();
            validationResponse = validator.Validate(request);
            if (validationResponse.IsValid)
                try
                {
                    var handler = new GetAuthorChatSessionsPaginatedQueryHandler(context);
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
        public void ThenTheResponseHasACollectionOfAuthorChatSessions()
        {
            if (responseType != CommandResponseType.Successful) return;
            _response?.TotalCount.Should().Be(_withinDateRangeExists == false ? 0 : _response.TotalCount);
        }

        [Then(@"Each chat session has a Key")]
        public void ThenEachChatSessionHasAKey()
        {
            if (responseType != CommandResponseType.Successful) return;
            _response?.Items.FirstOrDefault(x => x.Id == default).Should().BeNull();
        }

        [Then(@"Each chat session has a Date greater than start date")]
        public void ThenEachChatSessionHasADateGreaterThanStartDate()
        {
            if (responseType == CommandResponseType.Successful && _withinDateRangeExists)
                _response?.Items.FirstOrDefault(x => _startDate == default || x.Timestamp > _startDate).Should().NotBeNull();
        }

        [Then(@"Each chat session has a Date less than end date")]
        public void ThenEachChatSessionHasADateLessThanEndDate()
        {
            if (responseType == CommandResponseType.Successful && _withinDateRangeExists)
                _response?.Items.FirstOrDefault(x => _endDate == default || x.Timestamp < _endDate).Should().NotBeNull();
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
