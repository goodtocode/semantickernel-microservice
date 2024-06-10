using SemanticKernelMicroservice.Core.Application.Forecasts.Commands.Remove;
using SemanticKernelMicroservice.Core.Domain.Forecasts.Entities;

namespace SemanticKernelMicroservice.Specs.Application.Unit.Forecasts.Commands.Remove;

[Binding]
[Scope(Tag= "removeForecastCommand")]
public class RemoveForecastCommandStepDefinitions : TestBase
{
    private bool _forecastExists;
    private Guid _forecastKey;

    [Given(@"I have a def ""([^""]*)""")]
    public void GivenIHaveADef(string def)
    {
        _def = def;
    }

    [Given(@"I have a forecast key""([^""]*)""")]
    public void GivenIHaveAWeatherForecastKey(Guid key)
    {
        _forecastKey = key;
    }

    [Given(@"The forecast exists ""([^""]*)""")]
    public void GivenTheWeatherForecastExists(bool forecastExists)
    {
        _forecastExists = forecastExists;
    }


    [When(@"I remove the forecast")]
    public async Task WhenIRemoveTheWeatherForecast()
    {
        
        if (_forecastExists)
        {
            var zipcode = 99999;
            var weatherForecastAddValue =
                ForecastValue.Create(_forecastKey, DateTime.Now, 75, new List<int> { zipcode });
            var weatherForecast = new Forecast(weatherForecastAddValue.Value);
            SemanticKernelMicroserviceContext.Forecasts.Add(weatherForecast);
            await SemanticKernelMicroserviceContext.SaveChangesAsync(CancellationToken.None);
        }

        var request = new RemoveForecastCommand
        {
            Key = _forecastKey,
          
        };

        var validator = new RemoveForecastCommandValidator();
        _validationResponse = await validator.ValidateAsync(request);

        if (_validationResponse.IsValid)
            try
            {
                var handler = new RemoveForecastCommandHandler(SemanticKernelMicroserviceContext);
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