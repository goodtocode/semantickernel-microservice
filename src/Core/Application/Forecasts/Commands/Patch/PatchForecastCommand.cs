//using Goodtocode.SemanticKernel.Core.Application.Abstractions;
//using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;

//namespace Goodtocode.SemanticKernel.Core.Application.Forecasts.Commands.Patch;

//public class PatchForecastCommand : IRequest
//{
//    public Guid Key { get; set; }
//    public DateTime? Date { get; set; }
//    public int? TemperatureC { get; set; }
//    public List<int>? Zipcodes { get; set; }
//}

//public class PatchWeatherForecastCommandHandler : IRequestHandler<PatchForecastCommand>
//{
//    private readonly ISemanticKernelMicroserviceContext _context;

//    public PatchWeatherForecastCommandHandler(ISemanticKernelMicroserviceContext context)
//    {
//        _context = context;
//    }

//    public async Task Handle(PatchForecastCommand request, CancellationToken cancellationToken)
//    {
//        var weatherForecast = _context.Forecasts.Find(request.Key);

//        if (weatherForecast == null)
//            throw new CustomNotFoundException();
        
//        if (request.Date != null)
//            weatherForecast.UpdateDate((DateTime) request.Date);

//        if (request.TemperatureC != null)
//            weatherForecast.UpdateTemperatureF((int) request.TemperatureC);

//        if (request.Zipcodes != null)
//            weatherForecast.UpdateZipcodes(request.Zipcodes);

//        _context.Forecasts.Update(weatherForecast);
//        await _context.SaveChangesAsync(CancellationToken.None);
//    }
//}