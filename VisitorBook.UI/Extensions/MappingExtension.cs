using VisitorBook.BL.Mapping;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountryDtos;
using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Dtos.RegionDtos;
using VisitorBook.Core.Dtos.RegisterApplicationDto;
using VisitorBook.Core.Dtos.RoleDtos;
using VisitorBook.Core.Dtos.SubRegionDtos;
using VisitorBook.Core.Dtos.UserDtos;
using VisitorBook.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Core.Entities;

namespace VisitorBook.UI.Extensions
{
    public static class MappingExtension
    {
        public static void AddMappingExt(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapProfile));

            services.AddScoped<IPropertyMappingCollection,
                CityToCityGetResponseDtoMappingCollection<City, CityResponseDto>>();

            services.AddScoped<IPropertyMappingCollection,
                CountyToCountyGetResponseDtoMappingCollection<County, CountyResponseDto>>();

            services.AddScoped<IPropertyMappingCollection,
                VisitedCountyToVisitedCountyGetResponseDtoMappingCollection<VisitedCounty, VisitedCountyResponseDto>>();

            services.AddScoped<IPropertyMappingCollection,
                RegionToRegionGetResponseDtoMappingCollection<Region, RegionResponseDto>>();

            services.AddScoped<IPropertyMappingCollection,
                RoleToRoleGetResponseDtoMappingCollection<Role, RoleResponseDto>>();

            services.AddScoped<IPropertyMappingCollection,
                SubRegionToSubRegionGetResponseDtoMappingCollection<SubRegion, SubRegionResponseDto>>();

            services.AddScoped<IPropertyMappingCollection,
                UserToUserGetResponseDtoMappingCollection<User, UserResponseDto>>();

            services.AddScoped<IPropertyMappingCollection,
                CountryToCountryGetResponseDtoMappingCollection<Country, CountryResponseDto>>();

            services.AddScoped<IPropertyMappingCollection,
                RegisterApplicationToRegisterApplicationGetResponseDtoMappingCollection<RegisterApplication, RegisterApplicationResponseDto>>();
        }
    }
}
