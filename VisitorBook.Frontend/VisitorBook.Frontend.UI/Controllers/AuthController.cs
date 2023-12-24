using Microsoft.AspNetCore.Mvc;

namespace VisitorBook.Frontend.UI.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }

        public IActionResult Register()
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

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
