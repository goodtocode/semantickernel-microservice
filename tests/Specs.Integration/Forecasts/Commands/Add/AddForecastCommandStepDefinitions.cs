//namespace SemanticKernelMicroservice.Specs.Application.Unit.Forecasts.Commands.Add;

//[Binding]
//[Scope(Tag = "addForecastCommand")]
//public class AddForecastCommandStepDefinitions : TestBase
//{  
//    private Guid _forecastkey;
//    private DateTime _forecastDate;
//    private int? _tempF;
//    private List<int> _zipcodes = new List<int>();
//    private bool _forecastExists;

//    [Given(@"I have a def ""([^""]*)""")]
//    public void GivenIHaveADef(string def)
//    {
//        _def = def;
//    }

//    [Given(@"I have a forecast key ""([^""]*)""")]
//    public void GivenIHaveAWeatherForecastKey(Guid forecastKey)
//    {
//        _forecastkey = forecastKey;
//    }

//    [Given(@"The forecast exists ""([^""]*)""")]
//    public void GivenTheForecastExists(bool weatherForecastExists)
//    {
//        _forecastExists = weatherForecastExists;
//    }

//    [Given(@"I have a forecast date ""([^""]*)""")]
//    public void GivenIHaveAForecastDate(DateTime forecastDate)
//    {
//        _forecastDate = forecastDate;
//    }

//    [Given(@"I have a forecast TemperatureF ""([^""]*)""")]
//    public void GivenIHaveAForecastTemperatureF(int? tempF)
//    {
//        _tempF = tempF;
//    }

//    [Given(@"I have a collection of Zipcodes ""([^""]*)""")]
//    public void GivenIHaveACollectionOfZipCodes(string zipCodes)
//    {
//        if (string.IsNullOrWhiteSpace(zipCodes)) return;

//        var zipcodes = zipCodes.Split(',');

//        foreach (var zipcode in zipcodes)
//        {
//            _zipcodes.Add(int.Parse(zipcode));
//        }
//    }

//    [When(@"I add the forecast")]
//    public async Task WhenIAddTheWeatherForecast()
//    {
//        if (_forecastExists)
//        {
//            var weatherForecastAddValue = ForecastValue.Create(_forecastkey, DateTime.Now, 75, _zipcodes);

//            var weatherForecast = new Forecast(weatherForecastAddValue.Value);

//            SemanticKernelMicroserviceContext.Forecasts.Add(weatherForecast);
//            await SemanticKernelMicroserviceContext.SaveChangesAsync(CancellationToken.None);
//        }

//        var request = new AddForecastCommand()
//        {
//            Key = _forecastkey,
//            Date = _forecastDate,
//            TemperatureF = _tempF,
//            Zipcodes = _zipcodes
//        };

//        var validator = new AddForecastCommandValidator();
//        _validationResponse = await validator.ValidateAsync(request);

//        if (_validationResponse.IsValid)
//        {
//            try
//            {
//                var handler = new AddForecastCommandHandler(SemanticKernelMicroserviceContext);
//                await handler.Handle(request, CancellationToken.None);
//                _responseType = CommandResponseType.Successful;
//            }
//            catch (Exception e)
//            {
//                HandleAssignResponseType(e);
//            }
//        }
//        else
//            _responseType = CommandResponseType.BadRequest;
//    }

//    [Then(@"The response is ""([^""]*)""")]
//    public void ThenTheResponseIs(string response)
//    {
//        HandleHasResponseType(response);
//    }

//    [Then(@"If the response has validation issues I see the ""([^""]*)"" in the response")]
//    public void ThenIfTheResponseHasValidationIssuesISeeTheInTheResponse(string expectedErrors)
//    {
//        HandleExpectedValidationErrorsAssertions(expectedErrors);
//    }
//}