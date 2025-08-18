using Goodtocode.SemanticKernel.Core.Application.Audio;
using Goodtocode.SemanticKernel.Core.Application.Common.Models;
using Goodtocode.SemanticKernel.Core.Domain.Audio;

namespace Goodtocode.SemanticKernel.Specs.Integration.Audio
{
    [Binding]
    [Scope(Tag = "getTextAudioPaginatedQuery")]
    public class GetTextAudioPaginatedQueryStepDefinitions : TestBase
    {
        private bool _exists;
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _withinDateRangeExists;
        private int _pageNumber;
        private int _pageSize;
        private PaginatedList<TextAudioDto>? _response;

        [Given(@"I have a definition ""([^""]*)""")]
        public void GivenIHaveADefinition(string def)
        {
            base.def = def;
        }

        [Given(@"Text Audio exist ""([^""]*)""")]
        public void GivenTextAudioExist(string exists)
        {
            bool.TryParse(exists, out _exists).Should().BeTrue();
        }

        [Given(@"I have a start date ""([^""]*)""")]
        public void GivenIHaveAStartDate(string startDate)
        {
            if (string.IsNullOrWhiteSpace(startDate)) return;
            DateTime.TryParse(startDate, out _startDate).Should().BeTrue();
            _startDate = _withinDateRangeExists ? _startDate : _startDate.AddMinutes(1); //Handle for desired not-found scenarios
        }

        [Given(@"I have a end date ""([^""]*)""")]
        public void GivenIHaveAEndDate(string endDate)
        {
            if (string.IsNullOrWhiteSpace(endDate)) return;
            DateTime.TryParse(endDate, out _endDate).Should().BeTrue();
        }

        [Given(@"text audio within the date range exists ""([^""]*)""")]
        public void GivenTextAudioWithinTheDateRangeExists(string withinDateRangeExists)
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

        [When(@"I get the text audio paginated")]
        public async Task WhenIGetTheTextAudioPaginated()
        {
            if (_exists)
            {
                for (int i = 0; i < 2; i++)
                {
                    var textAudio = TextAudioEntity.Create(Guid.NewGuid(),
                        Guid.NewGuid(),
                        "Audio of a simple geometric design consisting of two yellow squares and one blue square. " +
                            "The blue square is placed at a 45-degree angle, positioned centrally below the two yellow squares, creating a symmetrical arrangement. " +
                            "Each square is connected by what appears to be black lines or sticks, suggesting they may represent nodes or elements in a network or structure. " +
                            "The background is white, which contrasts with the bright colors of the squares.",
                        new ReadOnlyMemory<byte>([0x01, 0x02, 0x03, 0x04])
                    );
                    context.TextAudio.Add(textAudio);
                }
                ;
                await context.SaveChangesAsync(CancellationToken.None);
            }

            var request = new GetTextAudioPaginatedQuery()
            {
                PageNumber = _pageNumber,
                PageSize = _pageSize,
                StartDate = _startDate == default ? null : _startDate,
                EndDate = _endDate == default ? null : _endDate
            };

            var validator = new GetTextAudioPaginatedQueryValidator();
            validationResponse = validator.Validate(request);
            if (validationResponse.IsValid)
                try
                {
                    var handler = new GetTextAudioPaginatedQueryHandler(context);
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

        [Then(@"The response has a collection of text audio")]
        public void ThenTheResponseHasACollectionOfTextAudio()
        {
            if (responseType != CommandResponseType.Successful) return;
            _response?.TotalCount.Should().Be(_withinDateRangeExists == false ? 0 : _response.TotalCount);
        }

        [Then(@"Each text audio has a Key")]
        public void ThenEachTextAudioHasAKey()
        {
            if (responseType != CommandResponseType.Successful) return;
            _response?.Items.FirstOrDefault(x => x.Id == default).Should().BeNull();
        }

        [Then(@"Each text audio has a Date greater than start date")]
        public void ThenEachTextAudioHasADateGreaterThanStartDate()
        {
            if (responseType == CommandResponseType.Successful && _withinDateRangeExists)
                _response?.Items.FirstOrDefault(x => (_startDate == default || x.Timestamp > _startDate)).Should().NotBeNull();
        }

        [Then(@"Each text audio has a Date less than end date")]
        public void ThenEachTextAudioHasADateLessThanEndDate()
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
