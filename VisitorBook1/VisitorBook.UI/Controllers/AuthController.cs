using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.AuthDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Extensions;

namespace VisitorBook.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            returnUrl = returnUrl ?? Url.Action("Index", "Home", new { Area = "App" });

            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre yanlış");

                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginRequestDto.Password, loginRequestDto.RememberMe, true);

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 dakika boyunca giriş yapamazsınız." });

                return View();
            }

            ModelState.AddModelErrorList(new List<string>()
            {
                "Email veya şifre yanlış",
                $"(Başarısız giriş sayısı = {await _userManager.GetAccessFailedCountAsync(user)})"
            });

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var identityResult = await _userManager.CreateAsync(new User()
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Email
            }, registerRequestDto.PasswordConfirm);

            if (identityResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Üyelik kayıt işlemi başarıyla gerçekleşmiştir.";

                return RedirectToAction(nameof(Register));
            }

            ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());

            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto forgotPasswordRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordRequestDto.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Bu email adresine sahip kullanıcı bulunamamıştır.");

                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = user.Id, token = passwordResetToken }, HttpContext.Request.Scheme);

            await _emailService.SendResetPasswordEmail(passwordResetLink, user.Email);

            TempData["SuccessMessage"] = "Şifre yenileme linki eposta adresinize gönderilmiştir";

            return RedirectToAction(nameof(ForgotPassword));
        }

        public IActionResult RecoverPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var userId = TempData["userId"].ToString();
            var token = TempData["token"].ToString();

            if (userId == null || token == null)
            {
                throw new Exception("Bir hata meydana geldi");
            }

            var hasUser = await _userManager.FindByIdAsync(userId);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamamıştır");

                return View();
            }

            var result = await _userManager.ResetPasswordAsync(hasUser, token, resetPasswordRequestDto.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla yenilenmiştir.";
            }

            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());

                return View();
            }

            return View();
        }
    }
}
