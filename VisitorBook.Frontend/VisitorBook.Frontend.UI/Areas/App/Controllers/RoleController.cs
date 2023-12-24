using Microsoft.AspNetCore.Mvc;

namespace VisitorBook.Frontend.UI.Areas.Admin.Controllers
{
    [Area("App")]
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateRole()
        {
            return View();
        }

        public IActionResult EditRole()
        {
            return View();
        }

        public IActionResult DeleteRole()
        {
            return View();
        }

        public IActionResult AssignRoleToUser()
        {
            return View();
        }
    }
}
