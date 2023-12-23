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
    }
}
