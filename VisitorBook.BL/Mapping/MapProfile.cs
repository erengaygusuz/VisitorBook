using AutoMapper;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Core.Dtos.VisitorAddressDtos;
using VisitorBook.Core.Dtos.VisitorDtos;
using VisitorBook.Core.Entities;

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
                .ForMember(e => e.CityName, opts => opts.MapFrom(e => e.County.City.Name))
                .ForMember(e => e.CountyName, opts => opts.MapFrom(e => e.County.Name))
                .ForMember(e => e.VisitorNameSurname, opts => opts.MapFrom(e => e.Visitor.Name + " " + e.Visitor.Surname));

            CreateMap<VisitorAddressRequestDto, VisitorAddress>();

            CreateMap<VisitorAddress, VisitorAddressResponseDto>()
                .ForMember(e => e.CityId, opts => opts.MapFrom(e => e.County.CityId));

            CreateMap<VisitorRequestDto, Visitor>();

            CreateMap<Visitor, VisitorResponseDto>()
                .ForMember(e => e.VisitorAddress, opts => { opts.Condition(e => e.VisitorAddress != null); opts.MapFrom(e => e.VisitorAddress); });
        }
    }
}
