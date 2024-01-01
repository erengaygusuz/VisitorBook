using AutoMapper;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountryDtos;
using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Dtos.RegionDtos;
using VisitorBook.Core.Dtos.RoleDtos;
using VisitorBook.Core.Dtos.SubRegionDtos;
using VisitorBook.Core.Dtos.UserDtos;
using VisitorBook.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Core.Dtos.VisitorAddressDtos;
using VisitorBook.Core.Dtos.VisitorStatisticDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Enums;

namespace VisitorBook.BL.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<RegionRequestDto, Region>();
            CreateMap<Region, RegionResponseDto>();
            CreateMap<RegionResponseDto, Region>();

            CreateMap<SubRegionRequestDto, SubRegion>();
            CreateMap<SubRegion, SubRegionResponseDto>()
                .ForMember(e => e.Region, opts => opts.MapFrom(e => e.Region));
            CreateMap<SubRegionResponseDto, SubRegion>();

            CreateMap<CountryRequestDto, Country>();
            CreateMap<Country, CountryResponseDto>()
                .ForMember(e => e.SubRegion, opts => opts.MapFrom(e => e.SubRegion));
            CreateMap<CountryResponseDto, Country>();

            CreateMap<CityRequestDto, City>();
            CreateMap<City, CityResponseDto>()
                .ForMember(e => e.Country, opts => opts.MapFrom(e => e.Country));
            CreateMap<CityResponseDto, City>();

            CreateMap<CountyRequestDto, County>();
            CreateMap<County, CountyResponseDto>()
                .ForMember(e => e.City, opts => opts.MapFrom(e => e.City));
            CreateMap<CountyResponseDto, County>();

            CreateMap<VisitedCountyRequestDto, VisitedCounty>();
            CreateMap<VisitedCounty, VisitedCountyResponseDto>()
                .ForPath(e => e.County.City, opts => opts.MapFrom(e => e.County.City))
                .ForMember(e => e.County, opts => opts.MapFrom(e => e.County))
                .ForMember(e => e.User, opts => opts.MapFrom(e => e.User));
            CreateMap<VisitedCountyResponseDto, VisitedCounty>();

            CreateMap<UserAddressRequestDto, UserAddress>();
            CreateMap<UserAddress, UserAddressResponseDto>()
                .ForMember(e => e.CityId, opts => opts.MapFrom(e => e.County.CityId));
            CreateMap<UserAddressResponseDto, UserAddress>();

            CreateMap<UserRequestDto, User>()
                .ForPath(e => e.Gender, opts => opts.MapFrom(e => (Gender)Enum.Parse(typeof(Gender), e.Gender)))
                .ForMember(e => e.UserAddress, opts => 
                { 
                    opts.Condition(e => e.UserAddress == null || (e.UserAddress != null && e.UserAddress.CityId != 0 && e.UserAddress.CountyId != 0)); 
                    opts.MapFrom(e => e.UserAddress); 
                });
            CreateMap<User, UserResponseDto>()
                .ForMember(e => e.Gender, opts => opts.MapFrom(e => e.Gender.ToString()))
                .ForMember(e => e.UserAddress, opts => { opts.Condition(e => e.UserAddress != null); opts.MapFrom(e => e.UserAddress); });
            CreateMap<UserResponseDto, User>();

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

            CreateMap<RoleRequestDto, Role>();
            CreateMap<Role, RoleResponseDto>();
            CreateMap<RoleResponseDto, Role>();
        }
    }
}
