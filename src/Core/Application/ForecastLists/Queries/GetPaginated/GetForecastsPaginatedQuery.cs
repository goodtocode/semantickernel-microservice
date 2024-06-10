//using AutoMapper.QueryableExtensions;
//using Goodtocode.SemanticKernel.Core.Application.Abstractions;
//using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
//using Goodtocode.SemanticKernel.Core.Application.Common.Models;

//namespace Goodtocode.SemanticKernel.Core.Application.ForecastLists.Queries.GetPaginated;

//public class GetForecastsPaginatedQuery : IRequest<PaginatedList<ForecastPaginatedDto>>
//{
//    public int PageNumber { get; init; } = 1;
//    public int PageSize { get; init; } = 10;
//}

//public class
//    GetSemanticKernelMicroservicePaginatedQueryHandler : IRequestHandler<GetForecastsPaginatedQuery,
//        PaginatedList<ForecastPaginatedDto>>
//{
//    private readonly ISemanticKernelMicroserviceContext _context;
//    private readonly IMapper _mapper;

//    public GetSemanticKernelMicroservicePaginatedQueryHandler(ISemanticKernelMicroserviceContext context, IMapper mapper)
//    {
//        _context = context;
//        _mapper = mapper;
//    }

//    public async Task<PaginatedList<ForecastPaginatedDto>> Handle(GetForecastsPaginatedQuery request,
//        CancellationToken cancellationToken)
//    {
//        var paginatedSemanticKernelMicroservice = await _context.ForecastViews
//            .AsNoTracking()
//            .OrderByDescending(x => x.ForecastDate)
//            .ProjectTo<ForecastPaginatedDto>(_mapper.ConfigurationProvider)
//            .PaginatedListAsync(request.PageNumber, request.PageSize);

//        foreach (var item in paginatedGoodtocode.SemanticKernel.Items) item.TemperatureC = (item.TemperatureF - 32) * 5 / 9;

//        return paginatedSemanticKernelMicroservice;
//    }
//}