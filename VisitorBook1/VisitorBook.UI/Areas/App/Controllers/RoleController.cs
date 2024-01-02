﻿using Microsoft.AspNetCore.Identity;
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

        public RoleController(RazorViewConverter razorViewConverter,
            IStringLocalizer<Language> localization, RoleManager<Role> roleManager, RoleDataTablesOptions roleDataTableOptions,
            IMapper mapper, IValidator<RoleRequestDto> roleRequestDtoValidator)
        {
            _roleManager = roleManager;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _roleDataTableOptions = roleDataTableOptions;
            _mapper = mapper;
            _roleRequestDtoValidator = roleRequestDtoValidator;
        }

        [Authorize(Permissions.UserManagement.View)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Permissions.UserManagement.View)]
        [HttpPost]
        public IActionResult GetAll()
        {
            _roleDataTableOptions.SetDataTableOptions(Request);

            var result = _roleManager.Roles.Select(x => 

                new RoleResponseDto 
                { 
                    Id = x.Id,
                    Name = x.Name

                }).ToPagedList(_roleDataTableOptions.GetDataTablesOptions());

            return DataTablesResult(result);
        }

        [Authorize(Permissions.UserManagement.Create)]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Permissions.UserManagement.Edit)]
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
                validationResult.AddToModelState(ModelState);

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
                validationResult.AddToModelState(ModelState);

                return View(permissionViewModel);
            }

            var roleToUpdate = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == permissionViewModel.Role.Id);

            if (roleToUpdate == null)
            {
                throw new Exception("Güncellenecek rol bulunamamıştır.");
            }

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

            return View(permissionViewModel);
        }

        [Authorize(Permissions.UserManagement.Delete)]
        [HttpDelete]
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

            return BadRequest(new { message = _localization["Roles.Notification.UnSuccessfullDelete.Text"].Value });
        }

    }
}
