using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;
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

        public async Task<IActionResult> Index()
        { 
            throw new NotImplementedException();

            VisitorStatisticViewModel visitorStatisticViewModel = new VisitorStatisticViewModel()
            {
                GetHighestCountOfVisitedCityByVisitor = await _visitorStatisticService.GetHighestCountOfVisitedCityByVisitorAsync(),
                GetHighestCountOfVisitedCountyByVisitor = await _visitorStatisticService.GetHighestCountOfVisitedCountyByVisitorAsync(),
                GetLongestDistanceByVisitorOneTime = await _visitorStatisticService.GetLongestDistanceByVisitorOneTimeAsync(),
                GetLongestDistanceByVisitorAllTime = await _visitorStatisticService.GetLongestDistanceByVisitorAllTimeAsync()
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

        [Route("Error/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            return View(new ErrorViewModel { StatusCode = statusCode, OriginalPath = feature?.OriginalPath });
        }

        //public IActionResult Error(string messages = "")
        //{
        //    string messagesJson = Encoding.ASCII.GetString(Base64UrlTextEncoder.Decode(messages));

        //    if (!string.IsNullOrEmpty(messagesJson))
        //    {
        //        var errorMessages = JsonSerializer.Deserialize<ErrorViewModel>(messagesJson);

        //        return View(errorMessages);
        //    }

        //    return RedirectToAction(nameof(Index));
        //}
    }
}