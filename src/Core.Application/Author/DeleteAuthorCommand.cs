using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class DeleteAuthorCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteAuthorCommandHandler(ISemanticKernelContext context) : IRequestHandler<DeleteAuthorCommand>
{
    private readonly ISemanticKernelContext _context = context;

    public async Task Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var Author = _context.Authors.Find(request.Id);
        GuardAgainstNotFound(Author);

        _context.Authors.Remove(Author!);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void GuardAgainstNotFound(AuthorEntity? Author)
    {
        if (Author == null)
            throw new CustomNotFoundException("Author Not Found");
    }
}