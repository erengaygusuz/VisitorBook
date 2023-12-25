using Microsoft.EntityFrameworkCore;
using VisitorBook.Backend.Core.Abstract;
using VisitorBook.Backend.Core.Dtos.CityDtos;
using VisitorBook.Backend.Core.Dtos.CountyDtos;
using VisitorBook.Backend.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Backend.Core.Dtos.VisitorDtos;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Utilities;
using VisitorBook.Backend.DAL.Concrete;
using VisitorBook.Backend.DAL.Data;
using VisitorBook.Backend.BL.Concrete;
using VisitorBook.Backend.BL.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddScoped<IPropertyMappingCollection, VisitorToVisitorGetResponseDtoMappingCollection<Visitor, VisitorResponseDto>>();
builder.Services.AddScoped(typeof(VisitorStatisticService));
builder.Services.AddScoped(typeof(LocationHelper));
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped(typeof(IEmailService), typeof(EmailService));
builder.Services.AddScoped(typeof(TokenHelper));

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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

SeedDatabase();

app.MapControllers();

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}