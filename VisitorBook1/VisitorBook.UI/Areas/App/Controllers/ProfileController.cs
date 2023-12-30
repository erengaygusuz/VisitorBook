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
using VisitorBook.UI.ViewModels;

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
        public async Task<IActionResult> UpdateSecurityInfo(ProfileViewModel profileViewModel)
        {
            ModelState.Remove("UserGeneralInfo");
            ModelState.Remove("UserSecurityInfo.Email");
            ModelState.Remove("UserSecurityInfo.Username");

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = _localization["Profiles.SecurityTab.Message1.Text"].Value });
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var checkOldPassword = await _userManager.CheckPasswordAsync(user, profileViewModel.UserSecurityInfo.PasswordOld);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");

                return BadRequest(new { message = _localization["Profiles.SecurityTab.Message2.Text"].Value });
            }

            var resultChangePassword = await _userManager.ChangePasswordAsync(user, profileViewModel.UserSecurityInfo.PasswordOld, profileViewModel.UserSecurityInfo.PasswordNew);

            if (!resultChangePassword.Succeeded)
            {
                ModelState.AddModelErrorList(resultChangePassword.Errors.Select(x => x.Description).ToList());
            }

            await _userManager.UpdateSecurityStampAsync(user);

            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(user, profileViewModel.UserSecurityInfo.PasswordNew, true, false);

            return Json(new { message = _localization["Profiles.SecurityTab.Message3.Text"].Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateGeneralInfo(ProfileViewModel profileViewModel)
        {
            ModelState.Remove("UserSecurityInfo");

            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = _localization["Profiles.GeneralTab.Message1.Text"].Value });
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.Name = profileViewModel.UserGeneralInfo.Name;
            user.Surname = profileViewModel.UserGeneralInfo.Surname;
            user.BirthDate = profileViewModel.UserGeneralInfo.BirthDate;
            user.Gender = (Gender) Enum.Parse(typeof(Gender), profileViewModel.UserGeneralInfo.Gender);
            user.PhoneNumber = profileViewModel.UserGeneralInfo.PhoneNumber;

            var updateToUserResult = await _userManager.UpdateAsync(user);

            if (!updateToUserResult.Succeeded)
            {
                ModelState.AddModelErrorList(updateToUserResult.Errors);
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
