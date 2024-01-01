using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Dtos.ProfileDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Enums;
using VisitorBook.Core.Extensions;
using VisitorBook.UI.Languages;
using VisitorBook.Core.ViewModels;

namespace VisitorBook.UI.Areas.Admin.Controllers
{
    [Authorize]
    [Area("App")]
    public class ProfileController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<Language> _localization;

        public ProfileController(SignInManager<User> signInManager, UserManager<User> userManager, IStringLocalizer<Language> localization)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _localization = localization;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var profileViewModel = new ProfileViewModel
            {
                UserSecurityInfo = new UpdateSecurityInfoDto()
                {
                    Email = user.Email,
                    Username = user.UserName
                },
                UserGeneralInfo = new UpdateGeneralInfoDto
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender.ToString(),
                    PhoneNumber = user.PhoneNumber
                },
                GenderList = new List<string> { "Male", "Female" }
                   .Select(u => new SelectListItem
                   {
                       Text = _localization["Enum.Gender." + u + ".Text"].Value,
                       Value = u.ToString()
                   })
            };

            return View(profileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSecurityInfo(UpdateSecurityInfoDto updateSecurityInfoDto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var checkOldPassword = await _userManager.CheckPasswordAsync(user, updateSecurityInfoDto.PasswordOld);

            if (!checkOldPassword)
            {
                return BadRequest(new { message = _localization["Profiles.SecurityTab.Message2.Text"].Value });
            }

            var resultChangePassword = await _userManager.ChangePasswordAsync(user, updateSecurityInfoDto.PasswordOld, updateSecurityInfoDto.PasswordNew);

            if (!resultChangePassword.Succeeded)
            {
                ModelState.AddModelErrorList(resultChangePassword.Errors.Select(x => x.Description).ToList());

                return BadRequest(new { message = _localization["Profiles.SecurityTab.Message4.Text"].Value });
            }

            await _userManager.UpdateSecurityStampAsync(user);

            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(user, updateSecurityInfoDto.PasswordNew, true, false);

            return Json(new { message = _localization["Profiles.SecurityTab.Message3.Text"].Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateGeneralInfo(UpdateGeneralInfoDto updateGeneralInfoDto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.Name = updateGeneralInfoDto.Name;
            user.Surname = updateGeneralInfoDto.Surname;
            user.BirthDate = updateGeneralInfoDto.BirthDate;
            user.Gender = (Gender) Enum.Parse(typeof(Gender), updateGeneralInfoDto.Gender);
            user.PhoneNumber = updateGeneralInfoDto.PhoneNumber;

            var updateToUserResult = await _userManager.UpdateAsync(user);

            if (!updateToUserResult.Succeeded)
            {
                return BadRequest(new { message = _localization["Profiles.GeneralTab.Message1.Text"].Value });
            }

            await _userManager.UpdateSecurityStampAsync(user);

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, true);

            return Json(new { message = _localization["Profiles.GeneralTab.Message2.Text"].Value });
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
