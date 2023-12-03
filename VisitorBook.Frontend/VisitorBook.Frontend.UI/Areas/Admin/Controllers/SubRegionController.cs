using Microsoft.AspNetCore.Mvc;

namespace VisitorBook.Frontend.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubRegionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
