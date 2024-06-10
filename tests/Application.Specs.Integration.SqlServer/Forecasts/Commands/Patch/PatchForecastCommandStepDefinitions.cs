using SemanticKernelMicroservice.Core.Application.Forecasts.Commands.Patch;
using SemanticKernelMicroservice.Core.Domain.Forecasts.Entities;

namespace SemanticKernelMicroservice.Specs.Application.Unit.Forecasts.Commands.Patch;

[Binding]
[Scope(Tag= "patchForecastCommand")]
public class PatchForecastCommandStepDefinitions : TestBase
{
    private DateTime _forecastDate;
    private bool _forecastExists;
    private Guid _forecastKey;
    private int? _temperatureF;
    private List<int> _zipcodes = new List<int>();

    [Given(@"I have a def ""([^""]*)""")]
    public void GivenIHaveADef(string def)
    {
        _def = def;
    }

    [Given(@"I have a forecast key ""([^""]*)""")]
    public void GivenIHaveAWeatherForecastKey(Guid forecastKey)
    {
        _forecastKey = forecastKey;
    }

    [Given(@"I have a forecast date ""([^""]*)""")]
    public void GivenIHaveAWeatherForecastDate(DateTime forecastDate)
    {
        _forecastDate = forecastDate;
    }

    [Given(@"I have a forecast TemperatureF ""([^""]*)""")]
    public void GivenIHaveAWeatherForecastTemperatureF(string temperatureF)
    {
        if (temperatureF != string.Empty)
        {
            _temperatureF = int.Parse(temperatureF);
        }
    }

    [Given(@"I have a collection of Zipcodes ""([^""]*)""")]
    public void GivenIHaveACollectionOfZipCodes(string zipCodes)
    {
        if (zipCodes == string.Empty)
            return;

        var zipCodesParsed = zipCodes.Split(",");
        _zipcodes = zipCodesParsed.Select(int.Parse).ToList();
    }

    [Given(@"the forecast exists ""([^""]*)""")]
    public void GivenTheWeatherForecastExists(bool forecastExists)
    {
        _forecastExists = forecastExists;
    }

    [When(@"I patch the forecast")]
    public async Task WhenIUpdateTheWeatherForecast()
    {
        var request = new PatchForecastCommand()
        {
            Key = _forecastKey,
            Date = _forecastDate,
            TemperatureC = _temperatureF,
            Zipcodes = _zipcodes
        };

        if (_forecastExists)
        {
            var weatherForecastValue =
                ForecastValue.Create(_forecastKey, DateTime.Now.AddDays(-1), 75, new List<int>(){ 90000, 90002});

            SemanticKernelMicroserviceContext.Forecasts.Add(new Forecast(weatherForecastValue.Value));
            await SemanticKernelMicroserviceContext.SaveChangesAsync(CancellationToken.None);
        }

        var validator = new PatchForecastCommandValidator();
        _validationResponse = await validator.ValidateAsync(request);

        if (_validationResponse.IsValid)
            try
            {
                var handler = new PatchWeatherForecastCommandHandler(SemanticKernelMicroserviceContext);
                await handler.Handle(request, CancellationToken.None);
                _responseType = CommandResponseType.Successful;
            }
            catch (Exception e)
            {
                HandleAssignResponseType(e);
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
}