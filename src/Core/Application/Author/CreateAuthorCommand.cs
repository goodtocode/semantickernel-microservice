using Goodtocode.SemanticKernel.Core.Application.Abstractions;
using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Domain.Author;
using Goodtocode.SemanticKernel.Core.Domain.ChatCompletion;

namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class CreateAuthorCommand : IRequest<AuthorDto>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}

public class CreateAuthorCommandHandler(ISemanticKernelContext context, IMapper mapper) : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly ISemanticKernelContext _context = context;

    public async Task<AuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        
        GuardAgainstEmptyName(request?.Name);
        GuardAgainstIdExsits(_context.Authors, request!.Id);

        // Persist Author
        var Author = new AuthorEntity() { Id = request!.Id == Guid.Empty ? Guid.NewGuid() : request!.Id };
        _context.Authors.Add(Author);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new CustomValidationException(
            [
                new("Id", "Id already exists")
            ]);
        }

        // Return session
        AuthorDto returnValue;
        try
        {
            returnValue = _mapper.Map<AuthorDto>(Author);
        }
        catch (Exception)
        {
            throw new CustomValidationException(
            [
                new("Id", "Id already exists")
            ]);
        }
        return returnValue;
    }

    private static void GuardAgainstEmptyName(string? message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new CustomValidationException(
            [
                new("Name", "A name is required to get a response")
            ]);
    }

    private static void GuardAgainstIdExsits(DbSet<AuthorEntity> dbSet, Guid id)
    {
        if (dbSet.Any(x => x.Id == id))
            throw new CustomValidationException(
            [
                new("Id", "Id already exists")
            ]);
    }
}