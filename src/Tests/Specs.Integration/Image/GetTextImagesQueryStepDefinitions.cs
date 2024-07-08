using Goodtocode.SemanticKernel.Core.Application.Image;
using Goodtocode.SemanticKernel.Core.Domain.Image;
using System.Text;

namespace Goodtocode.SemanticKernel.Specs.Integration.Image;

[Binding]
[Scope(Tag = "getTextImagesQuery")]
public class GetTextImagesQueryStepDefinitions : TestBase
{
    private bool _exists;
    private bool _withinDateRangeExists;
    private DateTime _endDate;
    private DateTime _startDate;
    private ICollection<TextImageDto>? _response;

    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        _def = def;
    }

    [Given(@"Text Image exist ""([^""]*)""")]
    public void GivenTextImagesExist(string exists)
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

    [Given(@"text image within the date range exists ""([^""]*)""")]
    public void GivenTextImagesWithinTheDateRangeExists(string withinDateRangeExists)
    {
        bool.TryParse(withinDateRangeExists, out _withinDateRangeExists).Should().BeTrue();
    }

    [When(@"I get the text image")]
    public async Task WhenIGetTheTextImages()
    {
        if (_exists)
        {            
            for (int i = 0; i < 2; i++)
            {
                var textImage = new TextImageEntity()
                {
                    Id = Guid.NewGuid(),
                    Description = "Image of a simple geometric design consisting of two yellow squares and one blue square. " +
                        "The blue square is placed at a 45-degree angle, positioned centrally below the two yellow squares, creating a symmetrical arrangement. " +
                        "Each square is connected by what appears to be black lines or sticks, suggesting they may represent nodes or elements in a network or structure. " +
                        "The background is white, which contrasts with the bright colors of the squares.",
                    Width = 1024,
                    Height = 1024,
                    ImageBytes = [0x01, 0x02, 0x03, 0x04],
                    Timestamp = _startDate.AddSeconds(_withinDateRangeExists == true ? 1 : -1)
                };
                _context.TextImages.Add(textImage);
            };
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetTextImagesQuery()
        {
            StartDate = _startDate == default ? null : _startDate,
            EndDate = _endDate == default ? null : _endDate
        };

        var validator = new GetTextImagesQueryValidator();
        _validationResponse = validator.Validate(request);
        if (_validationResponse.IsValid)
            try
            {
                var handler = new GetTextImagesQueryHandler(_context, Mapper);
                _response = await handler.Handle(request, CancellationToken.None);
                _responseType = CommandResponseType.Successful;
            }
            catch (Exception e)
            {
                _responseType = HandleAssignResponseType(e);
            }
        else
            _responseType = CommandResponseType.BadRequest;
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

    [Then(@"The response has a collection of text image")]
    public void ThenTheResponseHasACollectionOfTextImages()
    {
        _response?.Count.Should().Be(_withinDateRangeExists == false ? 0 : _response.Count);
    }

    [Then(@"Each text image has a Key")]
    public void ThenEachTextImageHasAKey()
    {
        _response?.FirstOrDefault(x => x.Id == default).Should().BeNull();
    }

    [Then(@"Each text image has a Date greater than start date")]
    public void ThenEachTextImageHasADateGreaterThanStartDate()
    {
        if (_withinDateRangeExists)
            _response?.FirstOrDefault(x => (_startDate == default || x.Timestamp > _startDate)).Should().NotBeNull();
    }

    [Then(@"Each text image has a Date less than end date")]
    public void ThenEachTextImageHasADateLessThanEndDate()
    {
        if (_withinDateRangeExists)
            _response?.FirstOrDefault(x => (_endDate == default || x.Timestamp < _endDate)).Should().NotBeNull();
    }
}
