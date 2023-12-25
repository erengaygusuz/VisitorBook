using Microsoft.AspNetCore.Mvc;
using VisitorBook.Frontend.UI.Models.Inputs;
using VisitorBook.Frontend.UI.Services;

namespace VisitorBook.Frontend.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthApiService _authApiService;

        public AuthController(AuthApiService authApiService)
        {
            _authApiService = authApiService;    
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInput loginInput)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var loginOutput = await _authApiService.LoginAsync(loginInput);

            Response.Cookies.Append("X-Access-Token", loginOutput.AccessToken, new CookieOptions() 
            { 
                Expires = loginOutput.Expiration 
            });

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

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInput registerInput)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _authApiService.RegisterAsync(registerInput);

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
