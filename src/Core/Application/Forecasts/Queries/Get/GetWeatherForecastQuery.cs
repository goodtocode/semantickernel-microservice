//using Goodtocode.SemanticKernel.Core.Application.Abstractions;
//using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
//using Goodtocode.SemanticKernel.Core.Domain.Forecasts.Entities;

//namespace Goodtocode.SemanticKernel.Core.Application.Forecasts.Queries.Get;

//public class GetWeatherForecastQuery : IRequest<ForecastVm>
//{
//    public Guid Key { get; set; }
//}

//public class GetForecastQueryHandler : IRequestHandler<GetWeatherForecastQuery, ForecastVm>
//{
//    private readonly ISemanticKernelMicroserviceContext _context;
//    private readonly IMapper _mapper;

//    public GetForecastQueryHandler(ISemanticKernelMicroserviceContext context, IMapper mapper)
//    {
//        _context = context;
//        _mapper = mapper;
//    }

//    public async Task<ForecastVm> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
//    {
//        var forecast = await _context.ForecastViews.FindAsync(request.Key);
//        GuardAgainstForecastNotFound(forecast);

//        var weatherForecast = _mapper.Map<ForecastVm>(forecast);
//        weatherForecast.TemperatureC = (forecast.TemperatureF - 32) * 5 / 9;
        
//        return weatherForecast;
//    }

//    private static void GuardAgainstForecastNotFound(ForecastsView? forecast)
//    {
//        if (forecast == null)
//            throw new CustomNotFoundException("Forecast Not Found");
//    }
//}