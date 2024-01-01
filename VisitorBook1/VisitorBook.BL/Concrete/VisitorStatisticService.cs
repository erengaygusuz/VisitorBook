using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Utilities;

namespace VisitorBook.BL.Concrete
{
    public class VisitorStatisticService
    {
        private readonly IRepository<VisitedCounty> _visitedCountyRepository;
        private readonly LocationHelper _locationHelper;

        public VisitorStatisticService(IRepository<VisitedCounty> visitedCountyRepository, LocationHelper locationHelper)
        {
            _locationHelper = locationHelper;
            _visitedCountyRepository = visitedCountyRepository;
        }

        public async Task<Tuple<string, string>> GetHighestCountOfVisitedCountyByVisitorAsync()
        {
            var visitedCountyWithVisitorAndVisitorAddress = _visitedCountyRepository
                .GetAll(v => v.User.UserAddress != null, include: u => u.Include(a => a.County).Include(a => a.User));

            var groupedVisitedList = visitedCountyWithVisitorAndVisitorAddress.GroupBy(a => a.UserId);

            var tuple = new Tuple<string, string>("--", "0");

            if (groupedVisitedList.Count() > 0)
            {
                var highestCountOfVisitedCountyByVisitor = await groupedVisitedList.Select(a => new
                {
                    VisitorInfo = a.First().User.Name + " " + a.First().User.Surname,
                    CountOfDistinctVisitedCounty = a.Distinct().Count()
                }).OrderByDescending(a => a.CountOfDistinctVisitedCounty).ThenBy(b => b.VisitorInfo).FirstAsync();

                tuple = new Tuple<string, string>(highestCountOfVisitedCountyByVisitor.VisitorInfo, highestCountOfVisitedCountyByVisitor.CountOfDistinctVisitedCounty.ToString());
            }

            return tuple;
        }

        public async Task<Tuple<string, string>> GetHighestCountOfVisitedCityByVisitorAsync()
        {
            var visitedCountyWithVisitorAndVisitorAddress = _visitedCountyRepository
                .GetAll(v => v.User.UserAddress != null, include: u => u.Include(a => a.County).Include(a => a.User));

            var groupedVisitedList = visitedCountyWithVisitorAndVisitorAddress.GroupBy(a => a.UserId);

            var tuple = new Tuple<string, string>("--", "0");

            if (groupedVisitedList.Count() > 0)
            {
                var highestCountOfVisitedCityByVisitor = await groupedVisitedList.Select(a => new
                {
                    VisitorInfo = a.First().User.Name + " " + a.First().User.Surname,
                    CountOfDistinctVisitedCounty = a.GroupBy(c => c.County.City).Count()
                }).OrderByDescending(a => a.CountOfDistinctVisitedCounty).ThenBy(b => b.VisitorInfo).FirstAsync();

                tuple = new Tuple<string, string>(highestCountOfVisitedCityByVisitor.VisitorInfo, highestCountOfVisitedCityByVisitor.CountOfDistinctVisitedCounty.ToString());
            }

            return tuple;
        }

        public async Task<Tuple<string, string>> GetLongestDistanceByVisitorOneTimeAsync()
        {
            var visitedCountyWithVisitorAndVisitorAddress = _visitedCountyRepository
                .GetAll(v => v.User.UserAddress != null, include: u => u.Include(a => a.County).Include(a => a.User)
                .ThenInclude(a => a.UserAddress).ThenInclude(i => i.County));

            var groupedVisitedList = visitedCountyWithVisitorAndVisitorAddress.GroupBy(a => a.UserId);

            var tuple = new Tuple<string, string>("--", "0");

            if (groupedVisitedList.Count() > 0)
            {
                var longestDistanceWithVisitorInfo = (await groupedVisitedList.ToListAsync()).Select(a => new
                {
                    VisitorInfo = a.First().User.Name + " " + a.First().User.Surname,
                    LongestDistance = a.Max(visitedCounty => CalculateDistance(visitedCounty))
                }).OrderByDescending(a => a.LongestDistance).ThenBy(b => b.VisitorInfo).First();

                tuple = new Tuple<string, string>(longestDistanceWithVisitorInfo.VisitorInfo, String.Format("{0:0.##}", longestDistanceWithVisitorInfo.LongestDistance));
            }

            return tuple;
        }

        public async Task<Tuple<string, string>> GetLongestDistanceByVisitorAllTimeAsync()
        {
            var visitedCountyWithVisitorAndVisitorAddress = _visitedCountyRepository
                .GetAll(v => v.User.UserAddress != null, include: u => u.Include(a => a.County).Include(a => a.User)
                .ThenInclude(a => a.UserAddress).ThenInclude(i => i.County));

            var groupedVisitedList = visitedCountyWithVisitorAndVisitorAddress.GroupBy(a => a.UserId);

            var tuple = new Tuple<string, string>("--", "0");

            if (groupedVisitedList.Count() > 0)
            {
                var longestDistanceWithVisitorInfo = (await groupedVisitedList.ToListAsync()).Select(a => new
                {
                    VisitorInfo = a.First().User.Name + " " + a.First().User.Surname,
                    LongestDistance = a.Sum(visitedCounty => CalculateDistance(visitedCounty))
                }).OrderByDescending(a => a.LongestDistance).ThenBy(b => b.VisitorInfo).First();

                tuple = new Tuple<string, string>(longestDistanceWithVisitorInfo.VisitorInfo, String.Format("{0:0.##}", longestDistanceWithVisitorInfo.LongestDistance));
            }

            return tuple;
        }

        private double CalculateDistance(VisitedCounty visitedCounty)
        {
            var distance = _locationHelper.GetDistance(
                   new Location()
                   {
                       Latitude = visitedCounty.User.UserAddress.County.Latitude,
                       Longitude = visitedCounty.User.UserAddress.County.Longitude
                   },
                   new Location()
                   {
                       Latitude = visitedCounty.County.Latitude,
                       Longitude = visitedCounty.County.Longitude
                   }) / 1000;

            return distance;
        }
    }
}
