using Microsoft.AspNetCore.Mvc;

namespace VisitorBook.Frontend.UI.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }
    }
}
