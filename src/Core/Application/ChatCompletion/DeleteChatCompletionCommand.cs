//using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
//using Goodtocode.SemanticKernel.Core.Application.Common.Interfaces;

//namespace Goodtocode.SemanticKernel.Core.Application.ChatCompletion.Commands.Remove;

//public class DeleteChatSessionCommand : IRequest
//{
//    public Guid Key { get; set; }
//}

//public class DeleteChatSessionCommandHandler : IRequestHandler<DeleteChatSessionCommand>
//{
//    private readonly ISemanticKernelMicroserviceContext _context;

//    public DeleteChatSessionCommandHandler(ISemanticKernelMicroserviceContext context)
//    {
//        _context = context;
//    }

//    public async Task Handle(DeleteChatSessionCommand request, CancellationToken cancellationToken)
//    {
//        var weatherChatCompletion = _context.ChatCompletions.Find(request.Key);

//        if (weatherChatCompletion == null) throw new CustomNotFoundException();
//        _context.ChatCompletions.Remove(weatherChatCompletion);
//        await _context.SaveChangesAsync(cancellationToken);
//    }
//}