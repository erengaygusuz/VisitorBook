using AutoMapper;
using VisitorBook.Backend.Core.Dtos.CityDtos;
using VisitorBook.Backend.Core.Dtos.CountryDtos;
using VisitorBook.Backend.Core.Dtos.CountyDtos;
using VisitorBook.Backend.Core.Dtos.RegionDtos;
using VisitorBook.Backend.Core.Dtos.SubRegionDtos;
using VisitorBook.Backend.Core.Dtos.UserDtos;
using VisitorBook.Backend.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Backend.Core.Dtos.VisitorAddressDtos;
using VisitorBook.Backend.Core.Dtos.VisitorDtos;
using VisitorBook.Backend.Core.Dtos.VisitorStatisticDtos;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Enums;

namespace VisitorBook.Backend.BL.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<RegionRequestDto, Region>();

            CreateMap<Region, RegionResponseDto>();

            CreateMap<SubRegionRequestDto, SubRegion>();

            CreateMap<SubRegion, SubRegionResponseDto>()
                .ForMember(e => e.Region, opts => opts.MapFrom(e => e.Region));

            CreateMap<CountryRequestDto, Country>();

            CreateMap<Country, CountryResponseDto>()
                .ForMember(e => e.SubRegion, opts => opts.MapFrom(e => e.SubRegion));

            CreateMap<CityRequestDto, City>();

            CreateMap<City, CityResponseDto>()
                .ForMember(e => e.Country, opts => opts.MapFrom(e => e.Country));

            CreateMap<CountyRequestDto, County>();

            CreateMap<County, CountyResponseDto>()
                .ForMember(e => e.City, opts => opts.MapFrom(e => e.City));

            CreateMap<VisitedCountyRequestDto, VisitedCounty>();

            CreateMap<VisitedCounty, VisitedCountyResponseDto>()
                .ForPath(e => e.County.City, opts => opts.MapFrom(e => e.County.City))
                .ForMember(e => e.County, opts => opts.MapFrom(e => e.County))
                .ForMember(e => e.Visitor, opts => opts.MapFrom(e => e.Visitor));

            CreateMap<VisitorAddressRequestDto, VisitorAddress>();

            CreateMap<VisitorAddress, VisitorAddressResponseDto>()
                .ForMember(e => e.CityId, opts => opts.MapFrom(e => e.County.CityId));

            CreateMap<VisitorRequestDto, Visitor>();

            CreateMap<Visitor, VisitorResponseDto>()
                .ForMember(e => e.VisitorAddress, opts => { opts.Condition(e => e.VisitorAddress != null); opts.MapFrom(e => e.VisitorAddress); });

            CreateMap<UserRequestDto, User>()
                .ForPath(e => e.Gender, opts => opts.MapFrom(e => Enum.Parse(typeof(Gender), e.Gender)));

            CreateMap<User, UserResponseDto>()
                .ForMember(e => e.Gender, opts => opts.MapFrom(e => e.Gender.ToString()));

            CreateMap<Tuple<string, string>, HighestCountOfVisitedCountyByVisitorResponseDto>()
                .ForMember(e => e.VisitorInfo, opts => opts.MapFrom(e => e.Item1))
                .ForMember(e => e.CountOfDistinctVisitedCounty, opts => opts.MapFrom(e => e.Item2));

            CreateMap<Tuple<string, string>, HighestCountOfVisitedCityByVisitorResponseDto>()
                .ForMember(e => e.VisitorInfo, opts => opts.MapFrom(e => e.Item1))
                .ForMember(e => e.CountOfDistinctVisitedCity, opts => opts.MapFrom(e => e.Item2));

            CreateMap<Tuple<string, string>, LongestDistanceByVisitorOneTimeResponseDto>()
                .ForMember(e => e.VisitorInfo, opts => opts.MapFrom(e => e.Item1))
                .ForMember(e => e.LongestDistance, opts => opts.MapFrom(e => e.Item2));

            CreateMap<Tuple<string, string>, LongestDistanceByVisitorAllTimeResponseDto>()
                .ForMember(e => e.VisitorInfo, opts => opts.MapFrom(e => e.Item1))
                .ForMember(e => e.LongestDistance, opts => opts.MapFrom(e => e.Item2));
        }
    }
}
