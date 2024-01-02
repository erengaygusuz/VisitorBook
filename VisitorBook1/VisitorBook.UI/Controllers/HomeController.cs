using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.ViewModels;

namespace VisitorBook.UI.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IHomeFactStatisticService _homeFactStatisticService;

        public HomeController(IHomeFactStatisticService homeFactStatisticService)
        {
            _homeFactStatisticService = homeFactStatisticService;
        }

        public async Task<IActionResult> Index()
        {
            var countryCount = _homeFactStatisticService.GetTotalCountryCount();

            var visitedLocationCount = _homeFactStatisticService.GetTotalVisitedLocationCount();

            var visitorCount = await _homeFactStatisticService.GetTotalVisitorCountAsync();

            var userCount = _homeFactStatisticService.GetTotalUserCount();

            var homeFactViewModel = new HomeFactViewModel
            {
                CountryCount = countryCount,
                VisitedLocationCount = visitedLocationCount,
                VisitorCount = visitorCount,
                UserCount = userCount
            };

            return View(homeFactViewModel);
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Service()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetCulture(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(30) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}