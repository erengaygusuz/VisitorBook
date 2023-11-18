using Microsoft.EntityFrameworkCore;
using VisitorBook.BL.Concrete;
using VisitorBook.BL.Mapping;
using VisitorBook.BL.Services;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountyDtos;
using VisitorBook.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Core.Dtos.VisitorDtos;
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
builder.Services.AddScoped<IPropertyMappingCollection, VisitorToVisitorGetResponseDtoMappingCollection<Visitor, VisitorResponseDto>>();

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

app.MapControllers();

app.Run();
