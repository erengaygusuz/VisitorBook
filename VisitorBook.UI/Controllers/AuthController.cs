using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Constants;
using VisitorBook.Core.Dtos.AuthDtos;
using VisitorBook.Core.Dtos.RegisterApplicationDto;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Enums;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly INotyfService _notifyService;
        private readonly IStringLocalizer<Language> _localization;
        private readonly IValidator<LoginRequestDto> _loginRequestDtoValidator;
        private readonly IValidator<RegisterRequestDto> _registerRequestDtoValidator;
        private readonly IValidator<ForgotPasswordRequestDto> _forgotPasswordRequestDtoValidator;
        private readonly IValidator<ResetPasswordRequestDto> _resetPasswordRequestDtoValidator;
        private readonly IValidator<RegisterApplicationCreateRequestDto> _registerApplicationCreateRequestDtoValidator;
        private readonly IWebHostEnvironment _environment;
        private readonly IService<RegisterApplication> _registerApplicationService;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService, INotyfService notifyService,
            IStringLocalizer<Language> localization, IValidator<LoginRequestDto> loginRequestDtoValidator, 
            IValidator<RegisterRequestDto> registerRequestDtoValidator, IValidator<ForgotPasswordRequestDto> forgotPasswordRequestDtoValidator, 
            IValidator<ResetPasswordRequestDto> resetPasswordRequestDtoValidator, IWebHostEnvironment environment, 
            IValidator<RegisterApplicationCreateRequestDto> registerApplicationCreateRequestDtoValidator, IService<RegisterApplication> registerApplicationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _notifyService = notifyService;
            _localization = localization;
            _loginRequestDtoValidator = loginRequestDtoValidator;
            _registerRequestDtoValidator = registerRequestDtoValidator;
            _forgotPasswordRequestDtoValidator = forgotPasswordRequestDtoValidator;
            _resetPasswordRequestDtoValidator = resetPasswordRequestDtoValidator;
            _registerApplicationCreateRequestDtoValidator = registerApplicationCreateRequestDtoValidator;
            _environment = environment;
            _registerApplicationService = registerApplicationService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto, string returnUrl = null)
        {
            var validationResult = await _loginRequestDtoValidator.ValidateAsync(loginRequestDto);

            if (!validationResult.IsValid)
            {
                return View();
            }

            returnUrl = returnUrl ?? Url.Action("Index", "Home", new { Area = "App" });

            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

            if (user == null)
            {
                _notifyService.Error(_localization["Auth.Login.Message1.Text"].Value);

                return View();
            }

            if (!user.EmailConfirmed)
            {
                var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                if (userRole == AppRoles.VisitorRecorder)
                {
                    _notifyService.Error(_localization["Auth.Login.Message6.Text"].Value);

                    return View();
                }

                _notifyService.Error(_localization["Auth.Login.Message5.Text"].Value);

                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginRequestDto.Password, loginRequestDto.RememberMe, true);

            if (signInResult.Succeeded)
            {
                _notifyService.Success(_localization["Auth.Login.Message2.Text"].Value);

                _notifyService.RemoveAll();

                return Redirect(returnUrl);
            }

            if (signInResult.IsLockedOut)
            {
                _notifyService.Error(_localization["Auth.Login.Message3.Text"].Value);

                return View();
            }

            _notifyService.Error(string.Format(_localization["Auth.Login.Message4.Text"].Value, await _userManager.GetAccessFailedCountAsync(user)));

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            var validationResult = await _registerRequestDtoValidator.ValidateAsync(registerRequestDto);

            if (!validationResult.IsValid)
            {
                return View();
            }

            var identityResult = await _userManager.CreateAsync(new User()
            {
                Name = registerRequestDto.Name,
                Surname = registerRequestDto.Surname,
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Email,
                BirthDate = new DateTime(day: 1, month: 1, year: 1991, hour: 12, minute: 30, second: 50)

            }, registerRequestDto.PasswordConfirm);

            if (identityResult.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(registerRequestDto.Email);

                var result = await _userManager.AddToRoleAsync(user, AppRoles.Visitor);

                if (result.Succeeded)
                {
                    string accountConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var accountConfirmLink = Url.Action("RegisterConfirmation", "Auth", new { userId = user.Id, token = accountConfirmToken }, HttpContext.Request.Scheme);

                    var pathToFile = _environment.WebRootPath
                                    + Path.DirectorySeparatorChar.ToString()
                                    + "templates"
                                    + Path.DirectorySeparatorChar.ToString()
                                    + "email-template.html";

                    string[] text;

                    using (StreamReader streamReader = System.IO.File.OpenText(pathToFile))
                    {
                        text = streamReader.ReadToEnd().Split('@');

                        text[1] = _localization["Layout.Header.Title.Text"].Value;
                        text[3] = _localization["EmailTemplates.AccountConfirmation.Text1"].Value;
                        text[5] = user.Name + " " + user.Surname;
                        text[7] = _localization["EmailTemplates.AccountConfirmation.Text2"].Value;
                        text[9] = _localization["EmailTemplates.AccountConfirmation.Text3"].Value;
                        text[11] = accountConfirmLink;
                        text[13] = _localization["EmailTemplates.AccountConfirmation.Text4"].Value;
                        text[15] = _localization["EmailTemplates.AccountConfirmation.Text5"].Value;
                        text[17] = accountConfirmLink;
                        text[19] = accountConfirmLink;
                        text[21] = _localization["EmailTemplates.AccountConfirmation.Text6"].Value;
                        text[23] = _localization["EmailTemplates.AccountConfirmation.Text7"].Value;
                        text[25] = _localization["EmailTemplates.AccountConfirmation.Text8"].Value;
                        text[27] = _localization["EmailTemplates.AccountConfirmation.Text8"].Value;
                        text[29] = _localization["EmailTemplates.AccountConfirmation.Text9"].Value;
                        text[31] = _localization["EmailTemplates.AccountConfirmation.Text10"].Value;
                        text[33] = _localization["EmailTemplates.AccountConfirmation.Text10"].Value;
                        text[35] = _localization["EmailTemplates.AccountConfirmation.Text11"].Value;
                    }

                    var emailSendResult = await _emailService.SendEmail(user.Email, _localization["EmailTemplates.AccountConfirmation.Text12"].Value, String.Concat(text));

                    if (emailSendResult)
                    {
                        _notifyService.Success(_localization["Auth.Register.Message2.Text"].Value);

                        return View();
                    }

                    _notifyService.Error(_localization["Auth.Register.Message4.Text"].Value);

                    return View();
                }

                else
                {
                    _notifyService.Error(_localization["Auth.Register.Message4.Text"].Value);

                    return View();
                }
            }

            else
            {
                _notifyService.Error(_localization["Auth.Register.Message4.Text"].Value);

                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> RegisterConfirmation(string userId, string token, string returnUrl = null)
        {
            if (userId == null || token == null)
            {
                _notifyService.Error(_localization["Auth.Register.Message3.Text"].Value);

                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                _notifyService.Error(_localization["Auth.Register.Message3.Text"].Value);

                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                returnUrl = returnUrl ?? Url.Action("Login", "Auth");

                await _userManager.UpdateSecurityStampAsync(user);

                _notifyService.Success(_localization["Auth.Register.Message5.Text"].Value);

                return Redirect(returnUrl);
            }

            else
            {
                _notifyService.Error(_localization["Auth.Register.Message3.Text"].Value);

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto forgotPasswordRequestDto)
        {
            var validationResult = await _forgotPasswordRequestDtoValidator.ValidateAsync(forgotPasswordRequestDto);

            if (!validationResult.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(forgotPasswordRequestDto.Email);

            if (user == null)
            {
                _notifyService.Error(_localization["Auth.ForgotPassword.Message1.Text"].Value);

                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordResetLink = Url.Action("ResetPassword", "Auth", new { userId = user.Id, token = passwordResetToken }, HttpContext.Request.Scheme);

            var pathToFile = _environment.WebRootPath
                            + Path.DirectorySeparatorChar.ToString()
                            + "templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "email-template.html";

            string[] text;

            using (StreamReader streamReader = System.IO.File.OpenText(pathToFile))
            {
                text = streamReader.ReadToEnd().Split('@');

                text[1] = _localization["Layout.Header.Title.Text"].Value;
                text[3] = _localization["EmailTemplates.ForgotPassword.Text1"].Value;
                text[5] = user.Name + " " + user.Surname;
                text[7] = _localization["EmailTemplates.ForgotPassword.Text2"].Value;
                text[9] = _localization["EmailTemplates.ForgotPassword.Text3"].Value;
                text[11] = passwordResetLink;
                text[13] = _localization["EmailTemplates.ForgotPassword.Text4"].Value;
                text[15] = _localization["EmailTemplates.ForgotPassword.Text5"].Value;
                text[17] = passwordResetLink;
                text[19] = passwordResetLink;
                text[21] = _localization["EmailTemplates.ForgotPassword.Text6"].Value;
                text[23] = _localization["EmailTemplates.ForgotPassword.Text7"].Value;
                text[25] = _localization["EmailTemplates.ForgotPassword.Text8"].Value;
                text[27] = _localization["EmailTemplates.ForgotPassword.Text8"].Value;
                text[29] = _localization["EmailTemplates.ForgotPassword.Text9"].Value;
                text[31] = _localization["EmailTemplates.ForgotPassword.Text10"].Value;
                text[33] = _localization["EmailTemplates.ForgotPassword.Text10"].Value;
                text[35] = _localization["EmailTemplates.ForgotPassword.Text11"].Value;
            }

            var emailSendResult = await _emailService.SendEmail(user.Email, _localization["EmailTemplates.ForgotPassword.Text12"].Value, String.Concat(text));

            if (emailSendResult)
            {
                _notifyService.Success(_localization["Auth.ForgotPassword.Message2.Text"].Value);

                return View();
            }

            _notifyService.Error(_localization["Auth.ForgotPassword.Message3.Text"].Value);

            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            var validationResult = await _resetPasswordRequestDtoValidator.ValidateAsync(resetPasswordRequestDto);

            if (!validationResult.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            var userId = TempData["userId"].ToString();
            var token = TempData["token"].ToString();

            if (userId == null || token == null)
            {
                _notifyService.Error(_localization["Auth.ResetPassword.Message3.Text"].Value);

                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                _notifyService.Error(_localization["Auth.ResetPassword.Message3.Text"].Value);

                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordRequestDto.Password);

            if (!result.Succeeded)
            {
                _notifyService.Error(_localization["Auth.ResetPassword.Message3.Text"].Value);

                return RedirectToAction("Index", "Home");
            }

            _notifyService.Success(_localization["Auth.ResetPassword.Message2.Text"].Value);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult RegisterApplication()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterApplication(RegisterApplicationCreateRequestDto registerApplicationCreateRequestDto)
        {
            var validationResult = await _registerApplicationCreateRequestDtoValidator.ValidateAsync(registerApplicationCreateRequestDto);

            if (!validationResult.IsValid)
            {
                return View();
            }

            var identityResult = await _userManager.CreateAsync(new User()
            {
                Name = registerApplicationCreateRequestDto.Name,
                Surname = registerApplicationCreateRequestDto.Surname,
                UserName = registerApplicationCreateRequestDto.Username,
                Email = registerApplicationCreateRequestDto.Email,
                BirthDate = new DateTime(day: 1, month: 1, year: 1991, hour: 12, minute: 30, second: 50)

            }, "12345");

            if (identityResult.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(registerApplicationCreateRequestDto.Email);

                var result = await _userManager.AddToRoleAsync(user, AppRoles.VisitorRecorder);

                if (result.Succeeded)
                {
                    var registerApplicationResult =  await _registerApplicationService.AddAsync(new RegisterApplication
                    {
                        UserId = user.Id,
                        Explanation = registerApplicationCreateRequestDto.Explanation,
                        Status = RegisterApplicationStatus.Pending
                    });

                    if (registerApplicationResult)
                    {
                        _notifyService.Success(_localization["Auth.RegisterApplication.Message1.Text"].Value);

                        return View();
                    }

                    else
                    {
                        _notifyService.Error(_localization["Auth.RegisterApplication.Message2.Text"].Value);

                        return View();
                    }
                }

                else
                {
                    _notifyService.Error(_localization["Auth.RegisterApplication.Message2.Text"].Value);

                    return View();
                }
            }

            else
            {
                _notifyService.Error(_localization["Auth.RegisterApplication.Message2.Text"].Value);

                return View();
            }
        }
    }
}
