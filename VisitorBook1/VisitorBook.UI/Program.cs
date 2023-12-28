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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IPropertyMappingService), typeof(PropertyMappingService));
builder.Services.AddScoped(typeof(FakeDataGenerator));
builder.Services.AddScoped<IPropertyMappingCollection, CityToCityGetResponseDtoMappingCollection<City, CityResponseDto>>();
builder.Services.AddScoped<IPropertyMappingCollection, CountyToCountyGetResponseDtoMappingCollection<County, CountyResponseDto>>();
builder.Services.AddScoped<IPropertyMappingCollection, VisitedCountyToVisitedCountyGetResponseDtoMappingCollection<VisitedCounty, VisitedCountyResponseDto>>();
builder.Services.AddScoped(typeof(VisitorStatisticService));
builder.Services.AddScoped(typeof(LocationHelper));
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped(typeof(IEmailService), typeof(EmailService));

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

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

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
    opt.LogoutPath = new PathString("/Profile/Logout");
    opt.AccessDeniedPath = new PathString("/Profile/AccessDenied");
    opt.Cookie = cookieBuilder;
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;
});

var app = builder.Build();

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
