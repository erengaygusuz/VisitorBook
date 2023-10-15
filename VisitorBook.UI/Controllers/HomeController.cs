using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.BL.Services;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly VisitorStatisticService _visitorStatisticService;

        public HomeController(VisitorStatisticService visitorStatisticService) 
        { 
            _visitorStatisticService = visitorStatisticService;
        }

        public IActionResult Index()
        {
            VisitorStatisticViewModel visitorStatisticViewModel = new VisitorStatisticViewModel()
            {
                GetHighestCountOfVisitedCityByVisitor = _visitorStatisticService.GetHighestCountOfVisitedCityByVisitor(),
                GetHighestCountOfVisitedStateByVisitor = _visitorStatisticService.GetHighestCountOfVisitedStateByVisitor(),
                GetLongestDistanceByVisitorOneTime = _visitorStatisticService.GetLongestDistanceByVisitorOneTime(),
                GetLongestDistanceByVisitorAllTime = _visitorStatisticService.GetLongestDistanceByVisitorAllTime()
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