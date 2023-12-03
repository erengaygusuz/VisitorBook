using Microsoft.AspNetCore.Mvc;

namespace VisitorBook.Frontend.UI.Areas.Visitor.Controllers
{
    [Area("Visitor")]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
