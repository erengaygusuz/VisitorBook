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
using VisitorBook.Backend.BL.Services;

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

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection"));
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