//using Goodtocode.SemanticKernel.Core.Application.Abstractions;
//using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;

//namespace Goodtocode.SemanticKernel.Core.Application.Forecasts.Commands.Remove;

//public class RemoveForecastCommand : IRequest
//{
//    public Guid Key { get; set; }
//}

//public class RemoveForecastCommandHandler : IRequestHandler<RemoveForecastCommand>
//{
//    private readonly ISemanticKernelMicroserviceContext _context;

//    public RemoveForecastCommandHandler(ISemanticKernelMicroserviceContext context)
//    {
//        _context = context;
//    }

//    public async Task Handle(RemoveForecastCommand request, CancellationToken cancellationToken)
//    {
//        var weatherForecast = _context.Forecasts.Find(request.Key);

//        if (weatherForecast == null) throw new CustomNotFoundException();
//        _context.Forecasts.Remove(weatherForecast);
//        await _context.SaveChangesAsync(cancellationToken);
//    }
//}