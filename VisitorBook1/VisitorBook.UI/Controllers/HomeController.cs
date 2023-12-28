using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace VisitorBook.UI.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {          
            return View();
        }

        public async Task<IActionResult> Contact()
        {
            return View();
        }

        public async Task<IActionResult> Service()
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