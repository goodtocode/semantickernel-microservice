using Goodtocode.Domain.Types;

namespace Goodtocode.SemanticKernel.Core.Domain.Subject;

public class AuthorEntity : DomainEntity<AuthorEntity>
{
    public string Name { get; set; } = string.Empty;
} 
