//using Goodtocode.SemanticKernel.Core.Application.Forecasts.Commands.Add;
//using Goodtocode.SemanticKernel.Core.Application.Forecasts.Commands.Patch;
//using Goodtocode.SemanticKernel.Core.Application.Forecasts.Commands.Remove;
//using Goodtocode.SemanticKernel.Core.Application.Forecasts.Commands.Update;
//using Goodtocode.SemanticKernel.Core.Application.Forecasts.Queries.Get;
//using Goodtocode.SemanticKernel.Presentation.WebApi.Common;

//namespace Goodtocode.SemanticKernel.Presentation.WebApi.Forecasts;

//[ApiController]
//[ApiConventionType(typeof(DefaultApiConventions))]
//[Route("[controller]")]
//[ApiVersion("1.0")]
//public class ForecastsController : ApiControllerBase
//{
    
//    /// <summary>Get Weather Forecast Query</summary>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///        "Key": 60fb5e99-3a78-43df-a512-7d8ff498499e
//    ///        "api-version":  1.0
//    /// 
//    /// </remarks>
//    /// <returns>WeatherForecastVm</returns>
//    [HttpGet("{key}", Name = "GetWeatherForecastQuery")]
//    [ProducesResponseType(typeof(ForecastVm), StatusCodes.Status200OK)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    public async Task<ForecastVm> Get(Guid key)
//    {
//        return await Mediator.Send(new GetWeatherForecastQuery
//        {
//            Key = key
//        });
//    }


//    /// <summary>
//    /// Add Forecast Command
//    /// </summary>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///     HttpPost Body
//    ///     {
//    ///        "Key": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//    ///        "Date": "2023-06-08T23:32:05.256Z",
//    ///        "TemperatureC": 0,
//    ///        "Zipcodes": [ 92602, 92673 ]
//    ///     }
//    ///
//    ///     "version":  1.0
//    /// </remarks>
//    /// <param name="command"></param>
//    /// <returns></returns>
//    [HttpPost(Name = "AddForecastCommand")]
//    [ProducesResponseType(StatusCodes.Status201Created)]
//    [ProducesResponseType(StatusCodes.Status400BadRequest)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    public async Task<ActionResult> ProvisionUserCommand(AddForecastCommand command)
//    {
//        await Mediator.Send(command);

//        return CreatedAtAction(nameof(Get), new { command.Key }, command);
//    }

//    /// <summary>
//    /// Update Forecast Command
//    /// </summary>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///     HttpPut Body
//    ///     {
//    ///        "Key": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//    ///        "Date": "2023-06-08T23:32:05.256Z",
//    ///        "TemperatureC": 0,
//    ///        "Zipcodes": [ 92602, 92673 ]
//    ///     }
//    ///
//    ///     "version":  1.0
//    /// </remarks>
//    /// <param name="command"></param>
//    /// <returns>NoContent</returns>
//    [HttpPut(Name = "UpdateForecastCommand")]
//    [ProducesResponseType(StatusCodes.Status204NoContent)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    public async Task<ActionResult> ProvisionUserCommand(UpdateForecastCommand command)
//    {
//        await Mediator.Send(command);

//        return NoContent();
//    }

//    /// <summary>
//    /// Patch Forecast Command
//    /// </summary>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///     HttpPatch Body
//    ///     {
//    ///        "Key": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//    ///        "Date": "2023-06-08T23:32:05.256Z",
//    ///        "TemperatureC": 0,
//    ///        "Zipcodes": [ 92602, 92673 ]
//    ///     }
//    ///
//    ///     "version":  1.0
//    /// </remarks>
//    /// <param name="command"></param>
//    /// <returns>NoContent</returns>
//    [HttpPatch(Name = "PatchForecastCommand")]
//    [ProducesResponseType(StatusCodes.Status204NoContent)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    public async Task<ActionResult> ProvisionUserCommand(PatchForecastCommand command)
//    {
//        await Mediator.Send(command);

//        return NoContent();
//    }

//    /// <summary>Remove Forecast Command</summary>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///        "Key": 60fb5e99-3a78-43df-a512-7d8ff498499e
//    ///        "api-version":  1.0
//    /// 
//    /// </remarks>
//    /// <returns>NoContent</returns>
//    [HttpDelete("{key}", Name= "RemoveForecastCommand")]
//    [ProducesResponseType(StatusCodes.Status204NoContent)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    [ProducesDefaultResponseType]
//    public async Task<IActionResult> Remove(Guid key)
//    {
//        await Mediator.Send(new RemoveForecastCommand(){ Key = key});

//        return NoContent();
//    }

//}