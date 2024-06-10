using SemanticKernelMicroservice.Core.Application.ForecastCalculations.Queries.GetCelsiusToFahrenheit;

namespace SemanticKernelMicroservice.Specs.Application.Unit.TempCalculations.Queries.GetCelsiusToFahrenheit;

[Binding]
[Scope(Tag= "getCelsiusToFahrenheitCalculationConversionQuery")]
public class GetCelsiusToFahrenheitCalculationConversionQueryStepDefinitions : TestBase
{
    private int _celsiusValueToConvert;
    private int _response;

    [Given(@"I have an Celsius value to convert")]
    public void GivenIHaveAnCelsiusValueToConvert()
    {
        _celsiusValueToConvert = 0;
    }

    [When(@"I get a Celsius To Fahrenheit Calculation Conversion")]
    public async Task WhenIGetACelsiusToFahrenheitCalculationConversion()
    {

        var request = new GetCelsiusToFahrenheitCalculationConversionQuery
        {
            CelsiusValue = _celsiusValueToConvert
        };

        var validator = new GetCelsiusToFahrenheitCalculationConversionQueryValidator();
        var validationResponse = await validator.ValidateAsync(request);

        if (validationResponse.IsValid)
            try
            {
                var handler = new GetCelsiusToFahrenheitCalculationConversionQueryHandler();
                _response = await handler.Handle(request, CancellationToken.None);
                _responseType = CommandResponseType.Successful;
            }
            catch (Exception e)
            {
                HandleAssignResponseType(e);
            }
        else
            _responseType = CommandResponseType.BadRequest;
    }

    [Then(@"I receive a successful response")]
    public void ThenIReceiveASuccessfulResponse()
    {
        HandleHasResponseType("Successful");
    }

    [Then(@"The response is the correct calculation")]
    public void ThenTheResponseIsTheCorrectCalculation()
    {
        _response.Should().Be((int)(_celsiusValueToConvert * 9 / 5) + 32);
    }
}