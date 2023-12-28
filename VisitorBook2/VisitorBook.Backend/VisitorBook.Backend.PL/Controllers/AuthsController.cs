using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VisitorBook.Backend.Core.Abstract;
using VisitorBook.Backend.Core.Dtos.AuthDtos;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Extensions;

namespace VisitorBook.Backend.PL.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private readonly IEmailService _emailService;

        private readonly IAuthService _authService;

        public AuthsController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService, IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _authService = authService;
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

            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

            if (user == null)
            {
                return NotFound();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginRequestDto.Password, loginRequestDto.RememberMe, true);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };

            var result = new LoginResponseDto
            {
                AccessToken = _authService.GenerateAccessToken(claims),
                AccessTokenExpiration = DateTime.Now,
                RefreshToken = _authService.GenerateRefreshToken(),
                RefreshTokenExpiration = DateTime.Now.AddMinutes(3),
                IsSucceeded = signInResult.Succeeded,
                IsLockedOut = signInResult.IsLockedOut,
                AccessFailedCount = await _userManager.GetAccessFailedCountAsync(user)
            };

            await _authService.AddRefreshTokenAsync(user.UserName, result.RefreshToken, DateTime.Now.AddMinutes(3));

            var r = await HttpContext.GetTokenAsync("access_token");

            return Ok(result);
        }

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
                Name = registerRequestDto.Name,
                Surname = registerRequestDto.Surname,
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Email

            }, registerRequestDto.PasswordConfirm);

            if (identityResult.Succeeded)
            {
                return Ok();
            }

            ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());

            return BadRequest();
        }

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

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> GetToken(GetTokenRequestDto getTokenRequestDto)
        {
            if (getTokenRequestDto is null)
            {
                return BadRequest("Invalid client request");
            }

            string accessToken = getTokenRequestDto.AccessToken;
            string refreshToken = getTokenRequestDto.RefreshToken;

            var principal = _authService.GetPrincipalFromExpiredToken(accessToken);

            var username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }

            var newAccessToken = _authService.GenerateAccessToken(principal.Claims);

            return Ok(new GetTokenResponseDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = user.RefreshToken
            });
        }

        [HttpPut]
        public async Task<IActionResult> RevokeToken()
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return BadRequest();
            }

            await _authService.ResetRefreshTokenAsync(username);

            return NoContent();
        }
    }
}
