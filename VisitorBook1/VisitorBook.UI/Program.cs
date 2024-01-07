using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisitorBook.BL.Concrete;
using VisitorBook.BL.Mapping;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Core.Entities;
using VisitorBook.DAL.Concrete;
using VisitorBook.DAL.Data;
using VisitorBook.Core.Dtos.CountryDtos;
using VisitorBook.Core.Dtos.RegionDtos;
using VisitorBook.Core.Dtos.RoleDtos;
using VisitorBook.Core.Dtos.SubRegionDtos;
using VisitorBook.Core.Dtos.UserDtos;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using VisitorBook.Core.Dtos.AuthDtos;
using VisitorBook.UI.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using VisitorBook.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using VisitorBook.UI.Filters;
using VisitorBook.Core.Dtos.ContactMessageDtos;
using VisitorBook.UI.TokenProviders;
using Microsoft.AspNetCore.Components.Authorization;
using VisitorBook.Core.Dtos.RegisterApplicationDto;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IPropertyMappingService), typeof(PropertyMappingService));
builder.Services.AddScoped(typeof(IFakeDataService), typeof(FakeDataService));
builder.Services.AddScoped<IPropertyMappingCollection, CityToCityGetResponseDtoMappingCollection<City, CityResponseDto>>();
builder.Services.AddScoped<IPropertyMappingCollection, CountyToCountyGetResponseDtoMappingCollection<County, CountyResponseDto>>();
builder.Services.AddScoped<IPropertyMappingCollection, VisitedCountyToVisitedCountyGetResponseDtoMappingCollection<VisitedCounty, VisitedCountyResponseDto>>();
builder.Services.AddScoped<IPropertyMappingCollection, RegionToRegionGetResponseDtoMappingCollection<Region, RegionResponseDto>>();
builder.Services.AddScoped<IPropertyMappingCollection, RoleToRoleGetResponseDtoMappingCollection<Role, RoleResponseDto>>();
builder.Services.AddScoped<IPropertyMappingCollection, SubRegionToSubRegionGetResponseDtoMappingCollection<SubRegion, SubRegionResponseDto>>();
builder.Services.AddScoped<IPropertyMappingCollection, UserToUserGetResponseDtoMappingCollection<User, UserResponseDto>>();
builder.Services.AddScoped<IPropertyMappingCollection, CountryToCountryGetResponseDtoMappingCollection<Country, CountryResponseDto>>();
builder.Services.AddScoped<IPropertyMappingCollection, RegisterApplicationToRegisterApplicationGetResponseDtoMappingCollection<RegisterApplication, RegisterApplicationResponseDto>>();
builder.Services.AddScoped(typeof(IVisitorStatisticService), typeof(VisitorStatisticService));
builder.Services.AddScoped(typeof(LocationHelper));
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped(typeof(IEmailService), typeof(EmailService));
builder.Services.AddScoped(typeof(IHomeFactStatisticService), typeof(HomeFactStatisticService));

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.FromMinutes(30);
});

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection"));
});

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.Tokens.EmailConfirmationTokenProvider = "AccountConfirmationTokenProvider";
    options.Tokens.PasswordResetTokenProvider = "PasswordResetTokenProvider";

}).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders()
.AddTokenProvider<AccountConfirmationTokenProvider<User>>("AccountConfirmationTokenProvider")
.AddTokenProvider<PasswordResetTokenProvider<User>>("PasswordResetTokenProvider");

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
     opt.TokenLifespan = TimeSpan.FromMinutes(15));

builder.Services.Configure<AccountConfirmationTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.Configure<PasswordResetTokenProviderOptions>(opt =>
    opt.TokenLifespan = TimeSpan.FromMinutes(30));

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.AddControllersWithViews().AddViewLocalization().AddDataAnnotationsLocalization(opts =>
{
    opts.DataAnnotationLocalizerProvider = (type, factory) =>
    {
        var assemblyName = new AssemblyName(typeof(Language).GetTypeInfo().Assembly.FullName!);

        return factory.Create(nameof(Language), assemblyName.Name!);
    };
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en-US"),
        new CultureInfo("en-GB"),
        new CultureInfo("tr-TR")
    };

    options.DefaultRequestCulture = new RequestCulture(culture: "en-GB", uiCulture: "en-GB");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddScoped(typeof(RazorViewConverter));
builder.Services.AddScoped(typeof(RegionDataTablesOptions));
builder.Services.AddScoped(typeof(SubRegionDataTablesOptions));
builder.Services.AddScoped(typeof(CountryDataTablesOptions));
builder.Services.AddScoped(typeof(CityDataTablesOptions));
builder.Services.AddScoped(typeof(CountyDataTablesOptions));
builder.Services.AddScoped(typeof(VisitedCountyDataTablesOptions));
builder.Services.AddScoped(typeof(UserDataTablesOptions));
builder.Services.AddScoped(typeof(RoleDataTablesOptions));

builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookieBuilder = new CookieBuilder();
    cookieBuilder.Name = "VisitorBook";

    opt.LoginPath = new PathString("/Auth/Login");
    opt.LogoutPath = new PathString("/App/Profile/Logout");
    opt.AccessDeniedPath = new PathString("/App/Home/AccessDenied");
    opt.Cookie = cookieBuilder;
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;
});

builder.Services.AddNotyf(config => { config.DurationInSeconds = 5; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });

builder.Services.AddScoped<IValidator<LoginRequestDto>, LoginRequestDtoValidator>();
builder.Services.AddScoped<IValidator<RegisterRequestDto>, RegisterRequestDtoValidator>();
builder.Services.AddScoped<IValidator<ForgotPasswordRequestDto>, ForgotPasswordRequestDtoValidator>();
builder.Services.AddScoped<IValidator<ResetPasswordRequestDto>, ResetPasswordRequestDtoValidator>();
builder.Services.AddScoped<IValidator<CityViewModel>, CityViewModelValidator>();
builder.Services.AddScoped<IValidator<CountyViewModel>, CountyViewModelValidator>();
builder.Services.AddScoped<IValidator<CountryViewModel>, CountryViewModelValidator>();
builder.Services.AddScoped<IValidator<VisitedCountyViewModel>, VisitedCountyViewModelValidator>();
builder.Services.AddScoped<IValidator<UserViewModel>, UserViewModelValidator>();
builder.Services.AddScoped<IValidator<RoleRequestDto>, RoleRequestDtoValidator>();
builder.Services.AddScoped<IValidator<SubRegionViewModel>, SubRegionViewModelValidator>();
builder.Services.AddScoped<IValidator<RegionRequestDto>, RegionRequestDtoValidator>();
builder.Services.AddScoped<IValidator<FakeDataViewModel>, FakeDataViewModelValidator>();
builder.Services.AddScoped<IValidator<ContactMessageRequestDto>, ContactMessageRequestDtoValidator>();
builder.Services.AddScoped<IValidator<ProfileViewModel>, ProfileViewModelValidator>();
builder.Services.AddScoped<IValidator<RegisterApplicationRequestDto>, RegisterApplicationRequestDtoValidator>();

builder.Services.AddFluentValidation(fv => {
    fv.DisableDataAnnotationsValidation = true;
});

var app = builder.Build();

app.UseNotyf();

app.UseStatusCodePagesWithReExecute("/Error/{0}");

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

SeedDatabase();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}
