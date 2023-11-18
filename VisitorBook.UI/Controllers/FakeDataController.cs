using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Entities;
using VisitorBook.DAL.Data;

namespace VisitorBook.UI.Controllers
{
    public class FakeDataController : Controller
    {
        private readonly FakeDataGenerator _fakeDataGenerator;

        private readonly IService<City> _cityService;
        private readonly IService<County> _countyService;
        private readonly IService<Visitor> _visitorService;
        private readonly IService<VisitorAddress> _visitorAddressService;
        private readonly IService<VisitedCounty> _visitedCountyService;

        public FakeDataController(FakeDataGenerator fakeDataGenerator, 
            IService<City> cityService, IService<County> countyService, IService<Visitor> visitorService,
            IService<VisitorAddress> visitorAddressService, IService<VisitedCounty> visitedCountyService) 
        { 
            _fakeDataGenerator = fakeDataGenerator;

            _cityService = cityService;
            _countyService = countyService;
            _visitorService = visitorService;
            _visitorAddressService = visitorAddressService;
            _visitedCountyService = visitedCountyService;
        }

        [HttpGet]
        public async Task<string> Generate(int populateConstant)
        {
            _fakeDataGenerator.GenerateData(populateConstant);

            await _cityService.AddRangeAsync(_fakeDataGenerator.Cities);
            await _countyService.AddRangeAsync(_fakeDataGenerator.Counties);
            await _visitorService.AddRangeAsync(_fakeDataGenerator.Visitors);
            await _visitorAddressService.AddRangeAsync(_fakeDataGenerator.VisitorAddresses);
            await _visitedCountyService.AddRangeAsync(_fakeDataGenerator.VisitedCounties);

            return "All datas are added.";
        }
    }
}
