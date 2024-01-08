using FluentValidation;
using FluentValidation.AspNetCore;
using VisitorBook.Core.Dtos.AuthDtos;
using VisitorBook.Core.Dtos.ContactMessageDtos;
using VisitorBook.Core.Dtos.RegionDtos;
using VisitorBook.Core.Dtos.RegisterApplicationDto;
using VisitorBook.Core.Dtos.RoleDtos;
using VisitorBook.Core.ViewModels;
using VisitorBook.UI.Validators;

namespace VisitorBook.UI.Extensions
{
    public static class ValidationExtension
    {
        public static void AddValidationExt(this IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginRequestDto>, LoginRequestDtoValidator>();
            services.AddScoped<IValidator<RegisterRequestDto>, RegisterRequestDtoValidator>();
            services.AddScoped<IValidator<ForgotPasswordRequestDto>, ForgotPasswordRequestDtoValidator>();
            services.AddScoped<IValidator<ResetPasswordRequestDto>, ResetPasswordRequestDtoValidator>();
            services.AddScoped<IValidator<CityViewModel>, CityViewModelValidator>();
            services.AddScoped<IValidator<CountyViewModel>, CountyViewModelValidator>();
            services.AddScoped<IValidator<CountryViewModel>, CountryViewModelValidator>();
            services.AddScoped<IValidator<VisitedCountyViewModel>, VisitedCountyViewModelValidator>();
            services.AddScoped<IValidator<UserViewModel>, UserViewModelValidator>();
            services.AddScoped<IValidator<RoleRequestDto>, RoleRequestDtoValidator>();
            services.AddScoped<IValidator<SubRegionViewModel>, SubRegionViewModelValidator>();
            services.AddScoped<IValidator<RegionRequestDto>, RegionRequestDtoValidator>();
            services.AddScoped<IValidator<FakeDataViewModel>, FakeDataViewModelValidator>();
            services.AddScoped<IValidator<ContactMessageRequestDto>, ContactMessageRequestDtoValidator>();
            services.AddScoped<IValidator<ProfileViewModel>, ProfileViewModelValidator>();
            services.AddScoped<IValidator<RegisterApplicationCreateRequestDto>, RegisterApplicationCreateRequestDtoValidator>();
            services.AddScoped<IValidator<RegisterApplicationViewModel>, RegisterApplicationViewModelValidator>();

            services.AddFluentValidation(fv => {
                fv.DisableDataAnnotationsValidation = true;
            });
        }
    }
}
