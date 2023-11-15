using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.BL.Services;
using VisitorBook.UI.ViewModels;

namespace VisitorBook.UI.Controllers
{
    public class HomeController : Controller
    {
        //private readonly VisitorStatisticService _visitorStatisticService;

        public HomeController(/*VisitorStatisticService visitorStatisticService*/) 
        { 
            //_visitorStatisticService = visitorStatisticService;
        }

        public async Task<IActionResult> Index()
        {
            VisitorStatisticViewModel visitorStatisticViewModel = new VisitorStatisticViewModel()
            {
                GetHighestCountOfVisitedCityByVisitor = new Tuple<string, string>("", ""),/*await _visitorStatisticService.GetHighestCountOfVisitedCityByVisitorAsync(),*/
                GetHighestCountOfVisitedCountyByVisitor = new Tuple<string, string>("", ""),/*await _visitorStatisticService.GetHighestCountOfVisitedCountyByVisitorAsync(),*/
                GetLongestDistanceByVisitorOneTime = new Tuple<string, string>("", ""),/*await _visitorStatisticService.GetLongestDistanceByVisitorOneTimeAsync(),*/
                GetLongestDistanceByVisitorAllTime = new Tuple<string, string>("", "")/*await _visitorStatisticService.GetLongestDistanceByVisitorAllTimeAsync()*/
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