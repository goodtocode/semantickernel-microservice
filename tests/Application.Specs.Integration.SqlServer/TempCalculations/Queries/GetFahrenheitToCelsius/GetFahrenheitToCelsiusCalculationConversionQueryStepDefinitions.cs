//namespace SemanticKernelMicroservice.Specs.Application.Unit.TempCalculations.Queries.GetFahrenheitToCelsius;

//[Binding]
//[Scope(Tag= "getFahrenheitToCelsiusCalculationConversionQuery")]
//public class GetFahrenheitToCelsiusCalculationConversionQueryStepDefinitions : TestBase
//{
//    private int _fahrenheitValueToConvert;
//    private int _response;

//    [Given(@"I have an Fahrenheit value to convert")]
//    public void GivenIHaveAnFahrenheitValueToConvert()
//    {
//        _fahrenheitValueToConvert = 100;
//    }

//    [When(@"I get a Fahrenheit To Celsius Calculation Conversion")]
//    public async Task WhenIGetAFahrenheitToCelsiusCalculationConversion()
//    {
//        var request = new GetFahrenheitToCelsiusCalculationConversionQuery
//        {
//            FahrenheitValue = _fahrenheitValueToConvert
//        };

//        var validator = new GetFahrenheitToCelsiusCalculationConversionQueryValidator();
//        var validationResponse = await validator.ValidateAsync(request);

//        if (validationResponse.IsValid)
//            try
//            {
//                var handler = new GetFahrenheitToCelsiusCalculationConversionQueryHandler();
//                _response = await handler.Handle(request, CancellationToken.None);
//                _responseType = CommandResponseType.Successful;
//            }
//            catch (Exception e)
//            {
//                HandleAssignResponseType(e);
//            }
//        else
//            _responseType = CommandResponseType.BadRequest;
//    }

//    [Then(@"I receive a successful response")]
//    public void ThenIReceiveASuccessfulResponse()
//    {
//        HandleHasResponseType("Successful");
//    }

//    [Then(@"The response contains is the correct calculation")]
//    public void ThenTheResponseContainsIsTheCorrectCalculation()
//    {
//        _response.Should().Be((_fahrenheitValueToConvert - 32) * 5 / 9);
//    }
//}