//using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
//using Goodtocode.SemanticKernel.Core.Application.Common.Interfaces;

//namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletions.Commands.Update;

//public class UpdateChatSessionCommand : IRequest
//{
//    public Guid Key { get; set; }
//    public DateTime Date { get; set; }
//    public int? TemperatureF { get; set; }
//    public List<int> Zipcodes { get; set; }
//}

//public class UpdateWeatherChatSessionCommandHandler : IRequestHandler<UpdateChatSessionCommand>
//{
//    private readonly ISemanticKernelMicroserviceContext _context;

//    public UpdateWeatherChatSessionCommandHandler(ISemanticKernelMicroserviceContext context)
//    {
//        _context = context;
//    }

//    public async Task Handle(UpdateChatSessionCommand request, CancellationToken cancellationToken)
//    {
//        var weatherChatCompletion = _context.ChatCompletions.Find(request.Key);
//        if (weatherChatCompletion == null) throw new CustomNotFoundException();

//        weatherChatCompletion.UpdateDate(request.Date);
//        weatherChatCompletion.UpdateTemperatureF((int) request.TemperatureF);
//        weatherChatCompletion.UpdateZipcodes(request.Zipcodes);
//        _context.ChatCompletions.Update(weatherChatCompletion);
//        await _context.SaveChangesAsync(CancellationToken.None);
//    }
//}