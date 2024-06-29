using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Core.Domain.Author;

namespace Goodtocode.SemanticKernel.Core.Application.Author;

public class AuthorDto : IMapFrom<AuthorEntity>
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    public DateTime? ModifiedOn { get; private set; }
    public DateTime? DeletedOn { get; private set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AuthorEntity, AuthorDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            .ForMember(d => d.CreatedOn, opt => opt.MapFrom(s => s.CreatedOn))
            .ForMember(d => d.ModifiedOn, opt => opt.MapFrom(s => s.ModifiedOn))
            .ForMember(d => d.DeletedOn, opt => opt.MapFrom(s => s.DeletedOn));
    }
}
