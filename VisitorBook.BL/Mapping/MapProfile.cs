using AutoMapper;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Core.Dtos.VisitorDtos;
using VisitorBook.Core.Models;

namespace VisitorBook.BL.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<CityAddRequestDto, City>();
            CreateMap<CityUpdateRequestDto, City>();
            CreateMap<City, CityGetResponseDto>();

            CreateMap<CountyAddRequestDto, County>();
            CreateMap<CountyUpdateRequestDto, County>().ReverseMap();
            CreateMap<County, CountyGetResponseDto>();

            CreateMap<VisitedCountyAddRequestDto, VisitedCounty>();

            CreateMap<VisitedCounty, VisitedCountyUpdateRequestDto>()
                .ForPath(e => e.CityId, opts => opts.MapFrom(e => e.County.CityId));

            CreateMap<VisitedCountyUpdateRequestDto, VisitedCounty>();

            CreateMap<VisitedCounty, VisitedCountyGetResponseDto>()
                .ForPath(e => e.CityId, opts => opts.MapFrom(e => e.County.CityId));

            CreateMap<VisitorAddRequestDto, Visitor>()
                .ForPath(dest => dest.VisitorAddress.CountyId, src => src.MapFrom(e => e.CountyId));

            CreateMap<Visitor, VisitorUpdateRequestDto>()
                .ForPath(e => e.CountyId, opts => opts.MapFrom(e => e.VisitorAddress.CountyId))
                .ForPath(e => e.CityId, opts => opts.MapFrom(e => e.VisitorAddress.County.CityId))
                .ForPath(e => e.VisitorAddressId, opts => opts.MapFrom(e => e.VisitorAddress.Id));

            CreateMap<VisitorUpdateRequestDto, Visitor>()
                .ForPath(e => e.VisitorAddress.CountyId, opts => opts.MapFrom(e => e.CountyId));

            CreateMap<Visitor, VisitorGetResponseDto>();
        }
    }
}
