using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Frontend.UI.Services;
using VisitorBook.Frontend.UI.ViewModels;

namespace VisitorBook.Frontend.UI.Area.App.Controllers
{
    [Area("App")]
    public class HomeController : Controller
    {
        private readonly VisitorStatisticApiService _visitorStatisticApiService;

        public HomeController(VisitorStatisticApiService visitorStatisticApiService) 
        { 
            _visitorStatisticApiService = visitorStatisticApiService;
        }

        public async Task<IActionResult> Index()
        {
            VisitorStatisticViewModel visitorStatisticViewModel = new VisitorStatisticViewModel()
            {
                GetHighestCountOfVisitedCityByVisitor = await _visitorStatisticApiService.GetHighestCountOfVisitedCityByVisitorAsync(),
                GetHighestCountOfVisitedCountyByVisitor = await _visitorStatisticApiService.GetHighestCountOfVisitedCountyByVisitorAsync(),
                GetLongestDistanceByVisitorOneTime = await _visitorStatisticApiService.GetLongestDistanceByVisitorOneTimeAsync(),
                GetLongestDistanceByVisitorAllTime = await _visitorStatisticApiService.GetLongestDistanceByVisitorAllTimeAsync()
            };            

            return View(visitorStatisticViewModel);
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