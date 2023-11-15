using AutoMapper;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Core.Dtos.VisitorAddressDtos;
using VisitorBook.Core.Dtos.VisitorDtos;
using VisitorBook.Core.Models;

namespace VisitorBook.BL.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<CityRequestDto, City>();
            CreateMap<City, CityResponseDto>();

            CreateMap<CountyRequestDto, County>();
            CreateMap<County, CountyResponseDto>()
                .ForMember(e => e.CityName, opts => opts.MapFrom(e => e.City.Name));

            CreateMap<VisitedCountyRequestDto, VisitedCounty>();
            CreateMap<VisitedCounty, VisitedCountyResponseDto>()
                .ForMember(e => e.CityId, opts => opts.MapFrom(e => e.County.CityId));

            CreateMap<VisitedCountyRequestDto, VisitedCounty>();
            CreateMap<VisitedCounty, VisitedCountyResponseDto>()
                .ForMember(e => e.CityId, opts => opts.MapFrom(e => e.County.CityId))
                .ForMember(e => e.CityName, opts => opts.MapFrom(e => e.County.City.Name))
                .ForMember(e => e.CountyName, opts => opts.MapFrom(e => e.County.Name))
                .ForMember(e => e.VisitorName, opts => opts.MapFrom(e => e.Visitor.Name + " " + e.Visitor.Surname));

            CreateMap<VisitorRequestDto, Visitor>()
                .ForPath(dest => dest.VisitorAddress.CountyId, src => src.MapFrom(e => e.CountyId));
            CreateMap<Visitor, VisitorResponseDto>()
                .ForPath(e => e.CountyId, opts => opts.MapFrom(e => e.VisitorAddress.CountyId))
                .ForPath(e => e.CityId, opts => opts.MapFrom(e => e.VisitorAddress.County.CityId))
                .ForPath(e => e.VisitorAddressId, opts => opts.MapFrom(e => e.VisitorAddress.Id));

            CreateMap<VisitorAddressRequestDto, VisitorAddress>();
            CreateMap<VisitorAddress, VisitorAddressResponseDto>();
        }
    }
}
