﻿using AspNetCoreHero.ToastNotification.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.AuthDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Extensions;
using VisitorBook.UI.Languages;

namespace VisitorBook.UI.Controllers
{
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

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService, INotyfService notifyService,
            IStringLocalizer<Language> localization, IValidator<LoginRequestDto> loginRequestDtoValidator, 
            IValidator<RegisterRequestDto> registerRequestDtoValidator, IValidator<ForgotPasswordRequestDto> forgotPasswordRequestDtoValidator, 
            IValidator<ResetPasswordRequestDto> resetPasswordRequestDtoValidator)
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
        }

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
                validationResult.AddToModelState(ModelState);

                return View();
            }

            returnUrl = returnUrl ?? Url.Action("Index", "Home", new { Area = "App" });

            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

            if (user == null)
            {
                _notifyService.Error(_localization["Auth.Login.Message1.Text"].Value);

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
                validationResult.AddToModelState(ModelState);

                return View();
            }

            var identityResult = await _userManager.CreateAsync(new User()
            {
                Name = registerRequestDto.Name,
                Surname = registerRequestDto.Surname,
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Email

            }, registerRequestDto.PasswordConfirm);

            if (identityResult.Succeeded)
            {
                var userToAssignRole = await _userManager.FindByEmailAsync(registerRequestDto.Email);

                var result = await _userManager.AddToRoleAsync(userToAssignRole, "Visitor");

                if (result.Succeeded)
                {
                    _notifyService.Success(_localization["Auth.Register.Message2.Text"].Value);

                    return RedirectToAction(nameof(Register));
                }
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
            var validationResult = await _forgotPasswordRequestDtoValidator.ValidateAsync(forgotPasswordRequestDto);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);

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

            await _emailService.SendResetPasswordEmail(passwordResetLink, user.Email);

            _notifyService.Success(_localization["Auth.ForgotPassword.Message2.Text"].Value);

            return RedirectToAction(nameof(ForgotPassword));
        }

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
                validationResult.AddToModelState(ModelState);

                return View();
            }

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

                _notifyService.Error(_localization["Auth.ResetPassword.Message1.Text"].Value);

                return View();
            }

            var result = await _userManager.ResetPasswordAsync(hasUser, token, resetPasswordRequestDto.Password);

            if (result.Succeeded)
            {
                _notifyService.Success(_localization["Auth.ResetPassword.Message2.Text"].Value);
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
