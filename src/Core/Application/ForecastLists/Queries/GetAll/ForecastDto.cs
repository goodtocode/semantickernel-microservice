//using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
//using Goodtocode.SemanticKernel.Core.Domain.Forecasts.Entities;

//namespace Goodtocode.SemanticKernel.Core.Application.ForecastLists.Queries.GetAll;

//public class ForecastDto : IMapFrom<ForecastsView>
//{
//    public Guid Key { get; set; }

//    public int TemperatureF { get; set; }

//    public DateTime Date { get; set; }

//    public int TemperatureC { get; set; }

//    public string Summary { get; set; }

//    public string ZipCodes { get; set; }

//    public void Mapping(Profile profile)
//    {
//        profile.CreateMap<ForecastsView, ForecastDto>()
//            .ForMember(d => d.Key, opt => opt.MapFrom(s => s.Key))
//            .ForMember(d => d.Date, opt => opt.MapFrom(s => s.ForecastDate))
//            .ForMember(d => d.TemperatureC, opt => opt.MapFrom(s => s.TemperatureF))
//            .ForMember(d => d.Summary, opt => opt.MapFrom(s => s.Summary))
//            .ForMember(d => d.ZipCodes, opt => opt.MapFrom(s => s.ZipCodesSearch))
//            .ForMember(d => d.ZipCodes, opt => opt.NullSubstitute(""));
//    }
//}