using Microsoft.AspNetCore.Mvc;

namespace VisitorBook.Frontend.UI.Areas.Admin.Controllers
{
    [Area("App")]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UpdatePassword()
        {
            return View();
        }

        public IActionResult UpdateGeneralInfo()
        {
            return View();
        }
    }
}
