using Goodtocode.SemanticKernel.Core.Application.Common.Models;
using Goodtocode.SemanticKernel.Core.Application.TextGeneration;
using Goodtocode.SemanticKernel.Core.Domain.TextGeneration;

namespace Goodtocode.SemanticKernel.Specs.Integration.TextGeneration
{
    [Binding]
    [Scope(Tag = "getTextPromptsPaginatedQuery")]
    public class GetTextPromptsPaginatedQueryStepDefinitions : TestBase
    {
        private bool _exists;
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _withinDateRangeExists;
        private int _pageNumber;
        private int _pageSize;
        private PaginatedList<TextPromptDto>? _response;

        [Given(@"I have a definition ""([^""]*)""")]
        public void GivenIHaveADefinition(string def)
        {
            base.def = def;
        }

        [Given(@"Text Prompt exist ""([^""]*)""")]
        public void GivenTextPromptsExist(string exists)
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

        [Given(@"text prompt within the date range exists ""([^""]*)""")]
        public void GivenTextPromptsWithinTheDateRangeExists(string withinDateRangeExists)
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

        [When(@"I get the text prompt paginated")]
        public async Task WhenIGetTheTextPromptsPaginated()
        {
            if (_exists)
            {
                for (int i = 0; i < 2; i++)
                {
                    var textPrompt = TextPromptEntity.Create(Guid.NewGuid(), Guid.Empty, "Tell me a bedtime story");
                    textPrompt.TextResponses =
                        [
                            TextResponseEntity.Create(Guid.Empty, textPrompt.Id, "Once upon a time...")
                        ];
                    context.TextPrompts.Add(textPrompt);
                }
                ;
                await context.SaveChangesAsync(CancellationToken.None);
            }

            var request = new GetTextPromptsPaginatedQuery()
            {
                PageNumber = _pageNumber,
                PageSize = _pageSize,
                StartDate = _startDate == default ? null : _startDate,
                EndDate = _endDate == default ? null : _endDate
            };

            var validator = new GetTextPromptsPaginatedQueryValidator();
            validationResponse = validator.Validate(request);
            if (validationResponse.IsValid)
                try
                {
                    var handler = new GetTextPromptsPaginatedQueryHandler(context);
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

        [Then(@"The response has a collection of text prompt")]
        public void ThenTheResponseHasACollectionOfTextPrompts()
        {
            if (responseType != CommandResponseType.Successful) return;
            _response?.TotalCount.Should().Be(_withinDateRangeExists == false ? 0 : _response.TotalCount);
        }

        [Then(@"Each text prompt has a Key")]
        public void ThenEachTextPromptHasAKey()
        {
            if (responseType != CommandResponseType.Successful) return;
            _response?.Items.FirstOrDefault(x => x.Id == default).Should().BeNull();
        }

        [Then(@"Each text prompt has a Date greater than start date")]
        public void ThenEachTextPromptHasADateGreaterThanStartDate()
        {
            if (responseType == CommandResponseType.Successful && _withinDateRangeExists)
                _response?.Items.FirstOrDefault(x => (_startDate == default || x.Timestamp > _startDate)).Should().NotBeNull();
        }

        [Then(@"Each text prompt has a Date less than end date")]
        public void ThenEachTextPromptHasADateLessThanEndDate()
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
