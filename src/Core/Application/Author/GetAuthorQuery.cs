using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class GetAuthorQuery : IRequest<AuthorDto>
{
    public Guid AuthorId { get; set; }
}

public class GetAuthorQueryHandler(ISemanticKernelContext context, IMapper mapper) : IRequestHandler<GetAuthorQuery, AuthorDto>
{
    private readonly ISemanticKernelContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<AuthorDto> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
    {
        var Author = await _context.Authors.FindAsync([request.AuthorId, cancellationToken], cancellationToken: cancellationToken);
        GuardAgainstNotFound(Author);

        return _mapper.Map<AuthorDto>(Author);
    }

    private static void GuardAgainstNotFound(AuthorEntity? Author)
    {
        if (Author == null)
            throw new CustomNotFoundException("Author Not Found");
    }
}