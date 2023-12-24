using Microsoft.AspNetCore.Mvc;

namespace VisitorBook.Frontend.UI.Areas.Admin.Controllers
{
    [Area("App")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        public IActionResult EditUser()
        {
            return View();
        }

        public IActionResult DeleteUser()
        {
            return View();
        }
    }
}
