using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Extensions;
using VisitorBook.Core.Dtos.UserDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Attributes;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.UI.Areas.App.Controllers;
using VisitorBook.UI.ViewModels;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountyDtos;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Dtos.VisitorAddressDtos;
using AutoMapper;

namespace VisitorBook.UI.Areas.Admin.Controllers
{
    [Area("App")]
    public class UserController : BaseController
    {
        private readonly IService<County> _countyService;
        private readonly IService<City> _cityService;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly UserDataTablesOptions _userDataTableOptions;
        private readonly IMapper _mapper;

        public UserController(IService<County> countyService, IService<City> cityService,
            UserManager<User> userManager, IStringLocalizer<Language> localization,
            RazorViewConverter razorViewConverter,
            UserDataTablesOptions userDataTableOptions, IMapper mapper)
        {
            _countyService = countyService;
            _cityService = cityService;
            _userManager = userManager;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _userDataTableOptions = userDataTableOptions;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAll()
        {
            _userDataTableOptions.SetDataTableOptions(Request);

            var result = _userManager.Users.Select(x =>

                new UserResponseDto
                {
                    Id = x.Id,
                    Name = x.Name

                }).ToPagedList(_userDataTableOptions.GetDataTablesOptions());

            return DataTablesResult(result);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Add()
        {
            var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

            var visitorViewModel = new UserViewModel()
            {
                User = new UserRequestDto(),
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
                CountyList = new List<SelectListItem>()
            };

            return View(visitorViewModel);
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var userResponseDto = await _userManager.Users.Include(u => u.UserAddress).FirstOrDefaultAsync(u => u.Id == id);

            IEnumerable<CountyResponseDto> countyResponseDtos;

            var userAddressResponseDto = new UserAddressResponseDto();

            if (userResponseDto.UserAddress != null)
            {
                userAddressResponseDto = _mapper.Map<UserAddressResponseDto>(userResponseDto.UserAddress);

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

            var userViewModel = new UserViewModel()
            {
                User = new UserRequestDto()
                {
                    Id = id,
                    Name = userResponseDto.Name,
                    Surname = userResponseDto.Surname,
                    BirthDate = userResponseDto.BirthDate,
                    Gender = userResponseDto.Gender.ToString(),
                    UserAddress = userResponseDto.UserAddress != null ?
                    new UserAddressRequestDto()
                    {
                        Id = userAddressResponseDto.Id,
                        CityId = userAddressResponseDto.CityId,
                        CountyId = userAddressResponseDto.CountyId
                    }
                    :
                    new UserAddressRequestDto()
                },
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

            return View(userViewModel);
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                await _userManager.CreateAsync(_mapper.Map<User>(userViewModel.User));

                return Json(new { isValid = true, message = _localization["Users.Notification.Add.Text"].Value });
            }

            var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

            userViewModel.CityList = (cityResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   });

            userViewModel.GenderList = new List<string> { "Male", "Female" }
                   .Select(u => new SelectListItem
                   {
                       Text = _localization["Enum.Gender." + u + ".Text"].Value,
                       Value = u.ToString()
                   });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", userViewModel) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                await _userManager.UpdateAsync(_mapper.Map<User>(userViewModel.User));

                return Json(new { isValid = true, message = _localization["Users.Notification.Edit.Text"].Value });
            }

            var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

            userViewModel.CityList = (cityResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   });

            userViewModel.GenderList = new List<string> { "Male", "Female" }
                   .Select(u => new SelectListItem
                   {
                       Text = _localization["Enum.Gender." + u + ".Text"].Value,
                       Value = u.ToString()
                   });

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", userViewModel) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var userToDelete = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (userToDelete == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(userToDelete);

            if (result.Succeeded)
            {
                return Json(new { message = _localization["Users.Notification.SuccessfullDelete.Text"].Value });
            }

            return BadRequest(new { message = _localization["Users.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
