using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Dtos.ProfileDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Enums;
using VisitorBook.UI.Languages;
using VisitorBook.Core.ViewModels;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.VisitorAddressDtos;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Dtos.CountyDtos;
using AutoMapper;
using FluentValidation;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.Extensions.FileProviders;

namespace VisitorBook.UI.Areas.AppControllers
{
    [Authorize]
    [Area("App")]
    public class ProfileController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<Language> _localization;
        private readonly IService<City> _cityService;
        private readonly IMapper _mapper;
        private readonly IService<County> _countyService;
        private readonly IValidator<ProfileViewModel> _profileViewModelValidator;
        private readonly INotyfService _notifyService;
        private readonly IService<UserAddress> _userAddressService;
        private readonly IFileProvider _fileProvider;

        public ProfileController(SignInManager<User> signInManager, UserManager<User> userManager, IStringLocalizer<Language> localization,
            IService<City> cityService, IMapper mapper, IService<County> countyService, IValidator<ProfileViewModel> profileViewModelValidator, 
            INotyfService notifyService, IService<UserAddress> userAddressService, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _localization = localization;
            _cityService = cityService;
            _mapper = mapper;
            _countyService = countyService;
            _profileViewModelValidator = profileViewModelValidator;
            _notifyService = notifyService;
            _userAddressService = userAddressService;
            _fileProvider = fileProvider;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.Users.Include(u => u.UserAddress).ThenInclude(c => c.County).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            IEnumerable<CountyResponseDto> countyResponseDtos;

            var userAddressResponseDto = new UserAddressResponseDto();

            if (user.UserAddress != null)
            {
                userAddressResponseDto = _mapper.Map<UserAddressResponseDto>(user.UserAddress);

                countyResponseDtos = await _countyService.GetAllAsync<CountyResponseDto>(
                    orderBy: o => o.OrderBy(x => x.Name),
                    expression: u => u.CityId == userAddressResponseDto.CityId,
                    include: x => x.Include(c => c.City));
            }

            else
            {
                countyResponseDtos = new List<CountyResponseDto>();
            }

            var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

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
                UserAddress = user.UserAddress != null ?
                    new UserAddressRequestDto()
                    {
                        Id = userAddressResponseDto.Id,
                        CityId = userAddressResponseDto.CityId,
                        CountyId = userAddressResponseDto.CountyId
                    }
                    :
                    new UserAddressRequestDto(),
                GenderList = new List<string> { "Male", "Female" }
                   .Select(u => new SelectListItem
                   {
                       Text = _localization["Enum.Gender." + u + ".Text"].Value,
                       Value = u.ToString()
                   }),
                CityList = (cityResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   }),
                CountyList = (countyResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   })
            };

            return View(profileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSecurityInfo(ProfileViewModel profileViewModel)
        {
            var validationResult = await _profileViewModelValidator.ValidateAsync(profileViewModel, options =>
            {
                options.IncludeProperties("UserSecurityInfo");
            });

            if (!validationResult.IsValid)
            {
                var messages = validationResult.ToDictionary();

                TempData["PasswordOld"] = messages.Where(x => x.Key == "UserSecurityInfo.PasswordOld").FirstOrDefault().Value 
                    != null ? messages.Where(x => x.Key == "UserSecurityInfo.PasswordOld").FirstOrDefault().Value.FirstOrDefault() 
                    : "";
                TempData["PasswordNew"] = messages.Where(x => x.Key == "UserSecurityInfo.PasswordNew").FirstOrDefault().Value 
                    != null ? messages.Where(x => x.Key == "UserSecurityInfo.PasswordNew").FirstOrDefault().Value.FirstOrDefault() 
                    : "";
                TempData["PasswordNewConfirm"] = messages.Where(x => x.Key == "UserSecurityInfo.PasswordNewConfirm").FirstOrDefault().Value 
                    != null ? messages.Where(x => x.Key == "UserSecurityInfo.PasswordNewConfirm").FirstOrDefault().Value.FirstOrDefault() 
                    : "";

                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var checkOldPassword = await _userManager.CheckPasswordAsync(user, profileViewModel.UserSecurityInfo.PasswordOld);

            if (!checkOldPassword)
            {
                _notifyService.Error(_localization["Profiles.SecurityTab.Message2.Text"].Value);

                return RedirectToAction("Index");
            }

            var resultChangePassword = await _userManager.ChangePasswordAsync(user, profileViewModel.UserSecurityInfo.PasswordOld, profileViewModel.UserSecurityInfo.PasswordNew);

            if (!resultChangePassword.Succeeded)
            {
                _notifyService.Error(_localization["Profiles.SecurityTab.Message4.Text"].Value);

                return RedirectToAction("Index");
            }

            await _userManager.UpdateSecurityStampAsync(user);

            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(user, profileViewModel.UserSecurityInfo.PasswordNew, true, false);

            _notifyService.Success(_localization["Profiles.SecurityTab.Message3.Text"].Value);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateGeneralInfo(ProfileViewModel profileViewModel)
        {
            var validationResult = await _profileViewModelValidator.ValidateAsync(profileViewModel, options =>
            {
                options.IncludeProperties("UserGeneralInfo");
            });

            if (!validationResult.IsValid)
            {
                var messages = validationResult.ToDictionary();

                TempData["Name"] = messages.Where(x => x.Key == "UserGeneralInfo.Name").FirstOrDefault().Value 
                    != null ? messages.Where(x => x.Key == "UserGeneralInfo.Name").FirstOrDefault().Value.FirstOrDefault() 
                    : "";
                TempData["Surname"] = messages.Where(x => x.Key == "UserGeneralInfo.Surname").FirstOrDefault().Value 
                    != null ? messages.Where(x => x.Key == "UserGeneralInfo.Surname").FirstOrDefault().Value.FirstOrDefault() 
                    : "";
                TempData["BirthDate"] = messages.Where(x => x.Key == "UserGeneralInfo.BirthDate").FirstOrDefault().Value 
                    != null ? messages.Where(x => x.Key == "UserGeneralInfo.BirthDate").FirstOrDefault().Value.FirstOrDefault() 
                    : "";
                TempData["Gender"] = messages.Where(x => x.Key == "UserGeneralInfo.Gender").FirstOrDefault().Value 
                    != null ? messages.Where(x => x.Key == "UserGeneralInfo.Gender").FirstOrDefault().Value.FirstOrDefault() 
                    : "";
                TempData["PhoneNumber"] = messages.Where(x => x.Key == "UserGeneralInfo.PhoneNumber").FirstOrDefault().Value 
                    != null ? messages.Where(x => x.Key == "UserGeneralInfo.PhoneNumber").FirstOrDefault().Value.FirstOrDefault() 
                    : "";

                TempData["Picture"] = messages.Where(x => x.Key == "UserGeneralInfo.Picture").FirstOrDefault().Value
                    != null ? messages.Where(x => x.Key == "UserGeneralInfo.Picture").FirstOrDefault().Value.FirstOrDefault()
                    : "";

                var result = validationResult.ToDictionary();

                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.Name = profileViewModel.UserGeneralInfo.Name;
            user.Surname = profileViewModel.UserGeneralInfo.Surname;
            user.BirthDate = profileViewModel.UserGeneralInfo.BirthDate;
            user.Gender = (Gender) Enum.Parse(typeof(Gender), profileViewModel.UserGeneralInfo.Gender);
            user.PhoneNumber = profileViewModel.UserGeneralInfo.PhoneNumber;

            if (profileViewModel.UserGeneralInfo.Picture != null && profileViewModel.UserGeneralInfo.Picture.Length > 0)
            {
                var imgFolder = _fileProvider.GetDirectoryContents("wwwroot/img");

                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(profileViewModel.UserGeneralInfo.Picture.FileName)}";

                var newPicturePath = Path.Combine(imgFolder.First(x => x.Name == "profile-photos").PhysicalPath, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);

                await profileViewModel.UserGeneralInfo.Picture.CopyToAsync(stream);

                user.Picture = randomFileName;
            }

            var updateToUserResult = await _userManager.UpdateAsync(user);

            if (profileViewModel.UserAddress != null && profileViewModel.UserAddress.CountyId != 0 && profileViewModel.UserAddress.CityId != 0)
            {
                await _userAddressService.UpdateAsync(new UserAddressRequestDto
                {
                    Id = profileViewModel.UserAddress.Id,
                    UserId = user.Id,
                    CountyId = profileViewModel.UserAddress.CountyId
                });
            }

            if (!updateToUserResult.Succeeded)
            {
                _notifyService.Error(_localization["Profiles.GeneralTab.Message1.Text"].Value);

                return RedirectToAction("Index");
            }

            await _userManager.UpdateSecurityStampAsync(user);

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, true);

            _notifyService.Success(_localization["Profiles.GeneralTab.Message2.Text"].Value);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
