using Microsoft.AspNetCore.Identity;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Constants;
using VisitorBook.Core.Entities;

namespace VisitorBook.BL.Concrete
{
    public class HomeFactStatisticService : IHomeFactStatisticService
    {
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<VisitedCounty> _visitedCountyRepository;
        private readonly UserManager<User> _userManager;

        public HomeFactStatisticService(IRepository<Country> countryRepository, IRepository<VisitedCounty> visitedCountyRepository, UserManager<User> userManager)
        {
            _countryRepository = countryRepository;
            _userManager = userManager;
            _visitedCountyRepository = visitedCountyRepository;
        }

        public int GetTotalCountryCount()
        {
            return _countryRepository.GetAll().Count();
        }

        public int GetTotalUserCount()
        {
            return _userManager.Users.Count();
        }

        public int GetTotalVisitedLocationCount()
        {
            return _visitedCountyRepository.GetAll().GroupBy(x => x.CountyId).Count();
        }

        public async Task<int> GetTotalVisitorCountAsync()
        {
            return (await _userManager.GetUsersInRoleAsync(AppRoles.Visitor)).Count;
        }
    }
}
