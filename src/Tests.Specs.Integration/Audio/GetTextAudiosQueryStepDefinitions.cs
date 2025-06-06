using System.Text;
using Goodtocode.SemanticKernel.Core.Application.Audio;
using Goodtocode.SemanticKernel.Core.Domain.Audio;

namespace Goodtocode.SemanticKernel.Specs.Integration.Audio;

[Binding]
[Scope(Tag = "getTextAudiosQuery")]
public class GetTextAudiosQueryStepDefinitions : TestBase
{
    private bool _exists;
    private bool _withinDateRangeExists;
    private DateTime _endDate;
    private DateTime _startDate;
    private ICollection<TextAudioDto>? _response;

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

    [Given(@"text audio within the date range exists ""([^""]*)""")]
    public void GivenTextAudioWithinTheDateRangeExists(string withinDateRangeExists)
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

    [When(@"I get the text audio")]
    public async Task WhenIGetTheTextAudio()
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

        var request = new GetTextAudiosQuery()
        {
            StartDate = _startDate == default ? null : _startDate,
            EndDate = _endDate == default ? null : _endDate
        };

        var validator = new GetTextAudiosQueryValidator();
        validationResponse = validator.Validate(request);
        if (validationResponse.IsValid)
            try
            {
                var handler = new GetTextAudiosQueryHandler(context, Mapper);
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
        _response?.Count.Should().Be(_withinDateRangeExists == false ? 0 : _response.Count);
    }

    [Then(@"Each text audio has a Key")]
    public void ThenEachTextAudioHasAKey()
    {
        _response?.FirstOrDefault(x => x.Id == default).Should().BeNull();
    }

    [Then(@"Each text audio has a Date greater than start date")]
    public void ThenEachTextAudioHasADateGreaterThanStartDate()
    {
        if (_withinDateRangeExists)
            _response?.FirstOrDefault(x => (_startDate == default || x.Timestamp > _startDate)).Should().NotBeNull();
    }

    [Then(@"Each text audio has a Date less than end date")]
    public void ThenEachTextAudioHasADateLessThanEndDate()
    {
        if (_withinDateRangeExists)
            _response?.FirstOrDefault(x => (_endDate == default || x.Timestamp < _endDate)).Should().NotBeNull();
    }
}
