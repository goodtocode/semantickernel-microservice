//namespace SemanticKernelMicroservice.Specs.Application.Unit.Forecasts.Commands.Update;

//[Binding]
//[Scope(Tag= "updateForecastCommand")]
//public class PatchWeatherForecastCommandStepDefinitions : TestBase
//{
//    private DateTime _forecastDate;
//    private bool _forecastExists;
//    private Guid _forecastKey;
//    private int? _temperatureC;
//    private List<int> _zipcodes = new List<int>();

//    [Given(@"I have a def ""([^""]*)""")]
//    public void GivenIHaveADef(string def)
//    {
//        _def = def;
//    }

//    [Given(@"I have a forecast key ""([^""]*)""")]
//    public void GivenIHaveAWeatherForecastKey(Guid forecastKey)
//    {
//        _forecastKey = forecastKey;
//    }

//    [Given(@"I have a forecast date ""([^""]*)""")]
//    public void GivenIHaveAWeatherForecastDate(DateTime forecastDate)
//    {
//        _forecastDate = forecastDate;
//    }

//    [Given(@"I have a forecast TemperatureF ""([^""]*)""")]
//    public void GivenIHaveAWeatherForecastTemperatureF(string temperatureF)
//    {
//        if (temperatureF != string.Empty)
//        {
//            _temperatureC = int.Parse(temperatureF);
//        }
//    }

//    [Given(@"I have a collection of Zipcodes ""([^""]*)""")]
//    public void GivenIHaveACollectionOfZipCodes(string zipCodes)
//    {
//        if (zipCodes == string.Empty)
//            return;

//        var zipCodesParsed = zipCodes.Split(",");
//        _zipcodes = zipCodesParsed.Select(int.Parse).ToList();
//    }


//    [Given(@"the forecast exists ""([^""]*)""")]
//    public void GivenTheWeatherForecastExists(bool forecastExists)
//    {
//        _forecastExists = forecastExists;
//    }

//    [When(@"I update the forecast")]
//    public async Task WhenIUpdateTheWeatherForecast()
//    {
//        var request = new UpdateForecastCommand
//        {
//            Key = _forecastKey,
//            Date = _forecastDate,
//            TemperatureF= _temperatureC,
//            Zipcodes = _zipcodes
//        };

//        if (_forecastExists)
//        {
            
//            var weatherForecastValue =
//                ForecastValue.Create(_forecastKey, DateTime.Now.AddDays(-1), 75, new List<int>(){ 90000, 90002});

//            SemanticKernelMicroserviceContext.Forecasts.Add(new Forecast(weatherForecastValue.Value));
//            await SemanticKernelMicroserviceContext.SaveChangesAsync(CancellationToken.None);
//        }

//        var validator = new UpdateForecastCommandValidator();
//        _validationResponse = await validator.ValidateAsync(request);

//        if (_validationResponse.IsValid)
//            try
//            {
//                var handler = new UpdateWeatherForecastCommandHandler(SemanticKernelMicroserviceContext);
//                await handler.Handle(request, CancellationToken.None);
//                _responseType = CommandResponseType.Successful;
//            }
//            catch (Exception e)
//            {
//                HandleAssignResponseType(e);
//            }
//        else
//            _responseType = CommandResponseType.BadRequest;
//    }


//    [Then(@"The response is ""([^""]*)""")]
//    public void ThenTheResponseIs(string response)
//    {
//        var def = _def;
//        HandleHasResponseType(response);
//    }

//    [Then(@"If the response has validation issues I see the ""([^""]*)"" in the response")]
//    public void ThenIfTheResponseHasValidationIssuesISeeTheInTheResponse(string expectedErrors)
//    {
//        HandleExpectedValidationErrorsAssertions(expectedErrors);
//    }
//}