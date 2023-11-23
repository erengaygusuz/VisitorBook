﻿using AutoMapper;
using VisitorBook.Backend.Core.Dtos.CityDtos;
using VisitorBook.Backend.Core.Dtos.CountyDtos;
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
            CreateMap<CityRequestDto, City>();

            CreateMap<City, CityResponseDto>();

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

            CreateMap<VisitorRequestDto, Visitor>()
                .ForMember(e => e.Gender, opts => opts.MapFrom(e => Enum.Parse(typeof(Gender), e.Gender)));

            CreateMap<Visitor, VisitorResponseDto>()
                .ForMember(e => e.Gender, opts => opts.MapFrom(e => e.Gender.ToString()))
                .ForMember(e => e.VisitorAddress, opts => { opts.Condition(e => e.VisitorAddress != null); opts.MapFrom(e => e.VisitorAddress); });

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