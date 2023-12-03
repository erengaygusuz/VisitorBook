using Microsoft.AspNetCore.Mvc;

namespace VisitorBook.Frontend.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CountryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
