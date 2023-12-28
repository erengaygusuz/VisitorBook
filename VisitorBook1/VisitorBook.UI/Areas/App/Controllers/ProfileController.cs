using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Dtos.ProfileDtos;
using VisitorBook.Core.Dtos.UserDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Enums;
using VisitorBook.Core.Extensions;

namespace VisitorBook.UI.Areas.Admin.Controllers
{
    [Area("App")]
    public class ProfileController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public ProfileController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UpdatePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordRequestDto updatePasswordRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, updatePasswordRequestDto.PasswordOld);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
                return View();
            }

            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, updatePasswordRequestDto.PasswordOld, updatePasswordRequestDto.PasswordNew);

            if (!resultChangePassword.Succeeded)
            {
                ModelState.AddModelErrorList(resultChangePassword.Errors.Select(x => x.Description).ToList());
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);

            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, updatePasswordRequestDto.PasswordNew, true, false);

            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirilmiştir.";

            return View();
        }

        public async Task<IActionResult> UpdateGeneralInfo()
        {
            ViewBag.genderList = new SelectList(Enum.GetNames(typeof(Gender)));

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var userResponseDto = new UserResponseDto()
            {
                Name = currentUser.Name,
                Surname = currentUser.Surname,
                BirthDate = currentUser.BirthDate,
                Gender = currentUser.Gender.ToString()
            };

            return View(userResponseDto);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserRequestDto userRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            currentUser.Name = userRequestDto.Name;
            currentUser.Surname = userRequestDto.Surname;
            currentUser.BirthDate = userRequestDto.BirthDate;
            currentUser.Gender = (Gender) Enum.Parse(typeof(Gender), userRequestDto.Gender);

            var updateToUserResult = await _userManager.UpdateAsync(currentUser);

            if (!updateToUserResult.Succeeded)
            {
                ModelState.AddModelErrorList(updateToUserResult.Errors);

                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, true);

            TempData["SuccessMessage"] = "Kullanıcı bilgileri başarıyla güncellenmiştir.";

            var userResponseDto = new UserResponseDto()
            {
                Name = currentUser.Name,
                Surname = currentUser.Surname,
                BirthDate = currentUser.BirthDate,
                Gender = currentUser.Gender.ToString()
            };

            return View(userResponseDto);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
