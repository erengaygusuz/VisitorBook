﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Backend.Core.Abstract;
using VisitorBook.Backend.Core.Dtos.AuthDtos;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Extensions;

namespace VisitorBook.Backend.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private readonly IEmailService _emailService;

        public AuthsController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (loginRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var hasUser = await _userManager.FindByEmailAsync(loginRequestDto.Email);

            if (hasUser == null)
            {
                return NotFound();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, loginRequestDto.Password, loginRequestDto.RememberMe, true);

            if (signInResult.Succeeded)
            {
                return Ok();
            }

            if (signInResult.IsLockedOut)
            {
                return Unauthorized();
            }

            ModelState.AddModelErrorList(new List<string>()
            {
                "Email veya şifre yanlış",
                $"(Başarısız giriş sayısı = {await _userManager.GetAccessFailedCountAsync(hasUser)})"
            });

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            if (registerRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var identityResult = await _userManager.CreateAsync(new User()
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Email,
                PhoneNumber = registerRequestDto.Phone
            }, registerRequestDto.PasswordConfirm);

            if (identityResult.Succeeded)
            {
                return Ok();
            }

            ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto forgotPasswordRequestDto)
        {
            if (forgotPasswordRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var hasUser = await _userManager.FindByEmailAsync(forgotPasswordRequestDto.Email);

            if (hasUser == null)
            {
                return NotFound();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordResetLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, token = passwordResetToken }, HttpContext.Request.Scheme);

            await _emailService.SendResetPasswordEmail(passwordResetLink, hasUser.Email);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
        {
            if (resetPasswordRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var userId = "";
            var token = "";

            if (userId == null || token == null)
            {
                throw new Exception("Bir hata meydana geldi");
            }

            var hasUser = await _userManager.FindByIdAsync(userId);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamamıştır");

                return NotFound();
            }

            var result = await _userManager.ResetPasswordAsync(hasUser, token, resetPasswordRequestDto.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());

                return BadRequest();
            }
        }
    }
}