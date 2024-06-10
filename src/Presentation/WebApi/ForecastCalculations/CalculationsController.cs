//using Goodtocode.SemanticKernel.Core.Application.ForecastCalculations.Queries.GetCelsiusToFahrenheit;
//using Goodtocode.SemanticKernel.Core.Application.ForecastCalculations.Queries.GetFahrenheitToCelsius;
//using Goodtocode.SemanticKernel.Presentation.WebApi.Common;

//namespace Goodtocode.SemanticKernel.Presentation.WebApi.ForecastCalculations;

///// <summary>
///// Controller for Calculations
///// </summary>
//[ApiController]
//[ApiConventionType(typeof(DefaultApiConventions))]
//[Route("[controller]")]
//[ApiVersion("1.0")]
//public class CalculationsController : ApiControllerBase
//{

//    /// <summary>Get Celsius To Fahrenheit Calculation Conversion Query</summary>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///        "Key": 10
//    ///        "api-version":  1.0
//    /// 
//    /// </remarks>
//    /// <returns>int</returns>
//    [HttpGet("fahrenheit", Name = "GetCelsiusToFahrenheitCalculationConversionQuery")]
//    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    public async Task<int> GetCelsiusToFahrenheitCalculationConversionQuery(int celsiusValue)
//    {
//        return await Mediator.Send(new GetCelsiusToFahrenheitCalculationConversionQuery()
//        {
//            CelsiusValue = celsiusValue
//        });
//    }
    
//    /// <summary>Get Fahrenheit To Celsius Calculation Conversion Query</summary>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///        "Key": 10
//    ///        "api-version":  1.0
//    /// 
//    /// </remarks>
//    /// <returns>int</returns>
//    [HttpGet("celsius", Name = "GetFahrenheitToCelsiusCalculationConversionQuery")]
//    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    public async Task<int> GetFahrenheitToCelsiusCalculationConversionQuery(int fahrenheitValue)
//    {
//        return await Mediator.Send(new GetFahrenheitToCelsiusCalculationConversionQuery()
//        {
//            FahrenheitValue = fahrenheitValue
//        });
//    }
//}