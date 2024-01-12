using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Extensions;
using VisitorBook.Core.Dtos.UserDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.UI.Areas.App.Controllers;
using VisitorBook.Core.ViewModels;
using VisitorBook.Core.Dtos.CityDtos;
using VisitorBook.Core.Dtos.CountyDtos;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Dtos.VisitorAddressDtos;
using AutoMapper;
using VisitorBook.Core.Dtos.RoleDtos;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using VisitorBook.Core.Constants;
using AutoMapper.QueryableExtensions;

namespace VisitorBook.UI.Areas.AppControllers
{
    [Authorize]
    [Area("App")]
    public class UserController : BaseController
    {
        private readonly IService<UserAddress> _userAddressService;
        private readonly IService<County> _countyService;
        private readonly IService<City> _cityService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly UserDataTablesOptions _userDataTableOptions;
        private readonly IMapper _mapper;
        private readonly IValidator<UserViewModel> _userViewModelValidator;
        private readonly IPropertyMappingService _propertyMappingService;

        public UserController(IService<County> countyService, IService<City> cityService,
            UserManager<User> userManager, IStringLocalizer<Language> localization,
            RazorViewConverter razorViewConverter,
            UserDataTablesOptions userDataTableOptions, IMapper mapper, RoleManager<Role> roleManager,
            IValidator<UserViewModel> userViewModelValidator, IPropertyMappingService propertyMappingService, 
            IService<UserAddress> userAddressService)
        {
            _countyService = countyService;
            _cityService = cityService;
            _userManager = userManager;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _userDataTableOptions = userDataTableOptions;
            _mapper = mapper;
            _roleManager = roleManager;
            _userViewModelValidator = userViewModelValidator;
            _propertyMappingService = propertyMappingService;
            _userAddressService = userAddressService;
        }

        [Authorize(Permissions.UserManagement.View)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Permissions.UserManagement.View)]
        [HttpPost]
        public IActionResult GetAll()
        {
            _userDataTableOptions.SetDataTableOptions(Request);

            var propertyMappings = _propertyMappingService.GetMappings<User, UserResponseDto>();

            var result = _userManager.Users
                .ApplySearch(_userDataTableOptions.GetDataTablesOptions(), propertyMappings)
                .ApplySort(_userDataTableOptions.GetDataTablesOptions(), propertyMappings)
                .ProjectTo<UserResponseDto>(_mapper.ConfigurationProvider)
                .Select(x =>
                    new UserResponseDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Surname = x.Surname,
                        BirthDate = x.BirthDate,
                        Gender = _localization["Enum.Gender." + x.Gender.ToString() + ".Text"].Value

                    })
                .ToPagedList(_userDataTableOptions.GetDataTablesOptions());

            return DataTablesResult(result);
        }

        [Authorize(Permissions.UserManagement.Create)]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var cityResponseDtos = await _cityService.GetAllAsync<CityResponseDto>();

            var roleResponseDtos = await _roleManager.Roles.Select(x =>

                new RoleResponseDto
                {
                    Id = x.Id,
                    Name = x.Name

                }).ToListAsync();

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
                CountyList = new List<SelectListItem>(),
                RoleList = (roleResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   })
            };

            return View(visitorViewModel);
        }

        [Authorize(Permissions.UserManagement.Edit)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.Users.Include(u => u.UserAddress).ThenInclude(c => c.County).FirstOrDefaultAsync(u => u.Id == id);

            var userRoleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            var userRoleId = (await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == userRoleName)).Id;

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

            var roleResponseDtos = await _roleManager.Roles.Select(x =>

                new RoleResponseDto
                {
                    Id = x.Id,
                    Name = x.Name

                }).ToListAsync();

            var userViewModel = new UserViewModel()
            {
                User = new UserRequestDto()
                {
                    Id = id,
                    Email = user.Email,
                    Username = user.UserName,
                    Name = user.Name,
                    Surname = user.Surname,
                    BirthDate = user.BirthDate,
                    Gender = user.Gender.ToString(),
                    SecurityStamp = user.SecurityStamp
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
                RoleId = userRoleId,
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
                   }),
                RoleList = (roleResponseDtos)
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   })
            };

            return View(userViewModel);
        }

        [Authorize(Permissions.UserManagement.Create)]
        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(UserViewModel userViewModel)
        {
            var validationResult = await _userViewModelValidator.ValidateAsync(userViewModel);

            if (!validationResult.IsValid)
            {
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

                var roleResponseDtos = await _roleManager.Roles.Select(x =>

                    new RoleResponseDto
                    {
                        Id = x.Id,
                        Name = x.Name

                    }).ToListAsync();

                userViewModel.RoleList = (roleResponseDtos)
                       .Select(u => new SelectListItem
                       {
                           Text = u.Name,
                           Value = u.Id.ToString()
                       });

                return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", userViewModel) });
            }

            var user = _mapper.Map<User>(userViewModel.User);

            var result = await _userManager.CreateAsync(user, "12345");

            if (result.Succeeded)
            {
                var userToAssignRole = await _userManager.FindByEmailAsync(userViewModel.User.Email);

                userToAssignRole.SecurityStamp = _userManager.CreateSecurityTokenAsync(userToAssignRole).ToString();
                userToAssignRole.EmailConfirmed = true;

                await _userManager.UpdateAsync(userToAssignRole);

                if (userViewModel.UserAddress != null)
                {
                    await _userAddressService.AddAsync(new UserAddressRequestDto
                    {
                        UserId = userToAssignRole.Id,
                        CountyId = userViewModel.UserAddress.CountyId
                    });
                }

                var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == userViewModel.RoleId);

                await _userManager.AddToRoleAsync(userToAssignRole, role.Name);
            }

            return Json(new { isValid = true, message = _localization["Users.Notification.Add.Text"].Value });
        }

        [Authorize(Permissions.UserManagement.Edit)]
        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(UserViewModel userViewModel)
        {
            var validationResult = await _userViewModelValidator.ValidateAsync(userViewModel);

            if (!validationResult.IsValid)
            {
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

                var roleResponseDtos = await _roleManager.Roles.Select(x =>

                    new RoleResponseDto
                    {
                        Id = x.Id,
                        Name = x.Name

                    }).ToListAsync();

                userViewModel.RoleList = (roleResponseDtos)
                       .Select(u => new SelectListItem
                       {
                           Text = u.Name,
                           Value = u.Id.ToString()
                       });

                return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", userViewModel) });
            }

            var userToUpdate = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userViewModel.User.Id);

            _mapper.Map(userViewModel.User, userToUpdate);

            await _userManager.UpdateAsync(userToUpdate);

            if (userViewModel.UserAddress != null && userViewModel.UserAddress.CountyId != 0 && userViewModel.UserAddress.CityId != 0)
            {
                await _userAddressService.UpdateAsync(new UserAddressRequestDto
                {
                    Id = userViewModel.UserAddress.Id,
                    UserId = userViewModel.User.Id,
                    CountyId = userViewModel.UserAddress.CountyId
                });
            }

            var userRoles = await _userManager.GetRolesAsync(userToUpdate);

            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == userViewModel.RoleId);

            foreach (var userRole in userRoles)
            {
                await _userManager.RemoveFromRoleAsync(userToUpdate, userRole);
            }

            await _userManager.AddToRoleAsync(userToUpdate, role.Name);

            return Json(new { isValid = true, message = _localization["Users.Notification.Edit.Text"].Value });
        }

        [Authorize(Permissions.UserManagement.Delete)]
        [HttpGet]
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

            return Json(new { message = _localization["Users.Notification.UnSuccessfullDelete.Text"].Value });
        }
    }
}
