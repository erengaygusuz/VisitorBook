using VisitorBook.Core.Abstract;
using VisitorBook.Core.Entities;

namespace VisitorBook.BL.Concrete
{
    public class PlaceStatisticService : IPlaceStatisticService
    {
        private readonly IRepository<VisitedCounty> _visitedCountyRepository;
        private readonly IRepository<County> _countyRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<SubRegion> _subRegionRepository;
        private readonly IRepository<Region> _regionRepository;

        public PlaceStatisticService(IRepository<VisitedCounty> visitedCountyRepository, IRepository<County> countyRepository, IRepository<City> cityRepository,
            IRepository<Country> countryRepository, IRepository<SubRegion> subRegionRepository, IRepository<Region> regionRepository)
        {
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
            _subRegionRepository = subRegionRepository;
            _regionRepository = regionRepository;
            _countyRepository = countyRepository;
            _visitedCountyRepository = visitedCountyRepository;
        }

        public int GetTotalCityCount()
        {
            return _cityRepository.GetAll().Count();
        }

        public int GetTotalCountryCount()
        {
            return _countryRepository.GetAll().Count();
        }

        public int GetTotalCountyCount()
        {
            return _countyRepository.GetAll().Count();
        }

        public int GetTotalRegionCount()
        {
            return _regionRepository.GetAll().Count();
        }

        public int GetTotalSubRegionCount()
        {
            return _subRegionRepository.GetAll().Count();
        }

        public int GetTotalVisitedCountyCount()
        {
            return _visitedCountyRepository.GetAll().Count();
        }
    }
}
