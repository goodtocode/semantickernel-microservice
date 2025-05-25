using AutoMapper.QueryableExtensions;
using Goodtocode.SemanticKernel.Core.Application.Common.Models;

namespace Goodtocode.SemanticKernel.Core.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
    {
        var paginatedItems = new List<TDestination>();
        var paginatedList = new PaginatedList<TDestination>(paginatedItems, 0, pageNumber, pageSize);
        return paginatedList.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
    }

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();
}
