using Microsoft.AspNetCore.Mvc;

namespace VisitorBook.Frontend.UI.Areas.VisitorRecorder.Controllers
{
    [Area("VisitRecorder")]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
