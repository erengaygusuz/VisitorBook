using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Dtos.RoleDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Areas.App.Controllers;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using VisitorBook.Core.Constants;
using VisitorBook.Core.ViewModels;
using System.Security.Claims;
using VisitorBook.Core.Abstract;
using AutoMapper.QueryableExtensions;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace VisitorBook.UI.Areas.AppControllers
{
    [Authorize]
    [Area("App")]
    public class RoleController : BaseController
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly RoleManager<Role> _roleManager;
        private readonly RoleDataTablesOptions _roleDataTableOptions;
        private readonly IMapper _mapper;
        private readonly IValidator<RoleRequestDto> _roleRequestDtoValidator;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly INotyfService _notifyService;

        public RoleController(RazorViewConverter razorViewConverter,
            IStringLocalizer<Language> localization, RoleManager<Role> roleManager, RoleDataTablesOptions roleDataTableOptions,
            IMapper mapper, IValidator<RoleRequestDto> roleRequestDtoValidator, IPropertyMappingService propertyMappingService, INotyfService notifyService)
        {
            _roleManager = roleManager;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _roleDataTableOptions = roleDataTableOptions;
            _mapper = mapper;
            _roleRequestDtoValidator = roleRequestDtoValidator;
            _propertyMappingService = propertyMappingService;
            _notifyService = notifyService;
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
            _roleDataTableOptions.SetDataTableOptions(Request);

            var propertyMappings = _propertyMappingService.GetMappings<Role, RoleResponseDto>();

            var result = _roleManager.Roles
                .ApplySearch(_roleDataTableOptions.GetDataTablesOptions(), propertyMappings)
                .ApplySort(_roleDataTableOptions.GetDataTablesOptions(), propertyMappings)
                .ProjectTo<RoleResponseDto>(_mapper.ConfigurationProvider)
                .ToPagedList(_roleDataTableOptions.GetDataTablesOptions());

            return DataTablesResult(result);
        }

        [Authorize(Permissions.UserManagement.Create)]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Permissions.UserManagement.Edit)]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (role == null)
            {
                return NotFound();
            }

            var roleClaims = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();

            var allClaims = Permissions.GenerateAllPermissions();

            var allPermissions = allClaims.Select(p => new RoleViewModel { DisplayValue = p }).ToList();

            foreach (var permission in allPermissions)
            {
                if (roleClaims.Any(c => c == permission.DisplayValue))
                {
                    permission.IsSelected = true;
                }
            }

            var viewModel = new PermissionViewModel
            {
                Role = new RoleRequestDto
                {
                    Id = id,
                    Name = role.Name
                },
                RoleClaims = allPermissions
            };

            return View(viewModel);
        }

        [Authorize(Permissions.UserManagement.Create)]
        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(RoleRequestDto roleRequestDto)
        {
            var validationResult = await _roleRequestDtoValidator.ValidateAsync(roleRequestDto);

            if (!validationResult.IsValid)
            {
                return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", roleRequestDto) });
            }
                
            var roleToAdd = _mapper.Map<Role>(roleRequestDto);
  
            await _roleManager.CreateAsync(roleToAdd);

            return Json(new { isValid = true, message = _localization["Roles.Notification.Add.Text"].Value });  
        }

        [Authorize(Permissions.UserManagement.Edit)]
        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(PermissionViewModel permissionViewModel)
        {
            var validationResult = await _roleRequestDtoValidator.ValidateAsync(permissionViewModel.Role);

            if (!validationResult.IsValid)
            {
                TempData["ErrorMessage"] = validationResult.Errors.FirstOrDefault().ErrorMessage;

                return RedirectToAction("Edit", permissionViewModel.Role.Id);
            }

            var roleToUpdate = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == permissionViewModel.Role.Id);

            roleToUpdate.Name = permissionViewModel.Role.Name;

            await _roleManager.UpdateAsync(roleToUpdate);

            var roleClaims = await _roleManager.GetClaimsAsync(roleToUpdate);

            foreach (var claim in roleClaims)
            {
                await _roleManager.RemoveClaimAsync(roleToUpdate, claim);
            }

            var selectedClaims = permissionViewModel.RoleClaims.Where(c => c.IsSelected).ToList();

            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddClaimAsync(roleToUpdate, new Claim("Permission", claim.DisplayValue));
            }

            _notifyService.Success(_localization["Roles.Notification.Edit.Text"].Value);

            return RedirectToAction("Edit", permissionViewModel.Role.Id);
        }

        [Authorize(Permissions.UserManagement.Delete)]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var roleToDelete = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (roleToDelete == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(roleToDelete);

            if (result.Succeeded)
            {
                return Json(new { message = _localization["Roles.Notification.SuccessfullDelete.Text"].Value });
            }

            return Json(new { message = _localization["Roles.Notification.UnSuccessfullDelete.Text"].Value });
        }

    }
}
