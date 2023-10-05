using Microsoft.AspNetCore.Mvc;

namespace VisitorBook.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}