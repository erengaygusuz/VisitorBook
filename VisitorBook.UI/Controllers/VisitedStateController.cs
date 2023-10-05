using Microsoft.AspNetCore.Mvc;

namespace VisitorBook.UI.Controllers
{
    public class VisitedStateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddOrEdit(int id = 0)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            return View();
        }
    }
}
