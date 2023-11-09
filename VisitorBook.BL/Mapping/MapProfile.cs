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
            CreateMap<CityAddRequestDto, City>();
            CreateMap<CityUpdateRequestDto, City>();
            CreateMap<City, CityGetResponseDto>();

            CreateMap<CountyAddRequestDto, County>();
            CreateMap<CountyUpdateRequestDto, County>();
            CreateMap<County, CountyGetResponseDto>();

            CreateMap<VisitedCountyAddRequestDto, VisitedCounty>();
            CreateMap<VisitedCountyUpdateRequestDto, VisitedCounty>();
            CreateMap<VisitedCounty, VisitedCountyGetResponseDto>();

            CreateMap<VisitorAddressAddRequestDto, VisitorAddress>();
            CreateMap<VisitorAddressUpdateRequestDto, VisitorAddress>();
            CreateMap<VisitorAddress, VisitorAddressGetResponseDto>();

            CreateMap<VisitorAddRequestDto, Visitor>();
            CreateMap<VisitorUpdateRequestDto, Visitor>();
            CreateMap<Visitor, VisitorGetResponseDto>();
        }
    }
}
