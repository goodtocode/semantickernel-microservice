using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class UpdateAuthorCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class UpdateAuthorCommandHandler(ISemanticKernelContext context) : IRequestHandler<UpdateAuthorCommand>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var Author = _context.Authors.Find(request.Id);
        GuardAgainstNotFound(Author);


        _context.Authors.Update(Author!);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void GuardAgainstNotFound(AuthorEntity? Author)
    {
        if (Author == null)
            throw new CustomNotFoundException("Author Not Found");
    }
}