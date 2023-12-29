using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VisitorBook.Core.Dtos.RoleDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Utilities;
using VisitorBook.UI.Areas.App.Controllers;
using VisitorBook.UI.Attributes;
using VisitorBook.UI.Configurations;
using VisitorBook.UI.Languages;
using VisitorBook.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace VisitorBook.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("App")]
    public class RoleController : BaseController
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly RoleManager<Role> _roleManager;
        private readonly RoleDataTablesOptions _roleDataTableOptions;
        private readonly IMapper _mapper;

        public RoleController(RazorViewConverter razorViewConverter,
            IStringLocalizer<Language> localization, RoleManager<Role> roleManager, RoleDataTablesOptions roleDataTableOptions,
            IMapper mapper)
        {
            _roleManager = roleManager;
            _localization = localization;
            _razorViewConverter = razorViewConverter;
            _roleDataTableOptions = roleDataTableOptions;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

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

        [NoDirectAccess]
        public IActionResult Add()
        {
            return View();
        }

        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {
            var roleResponseDto = await _roleManager.Roles.Select(x =>

                new RoleResponseDto
                {
                    Id = x.Id,
                    Name = x.Name

                }).FirstOrDefaultAsync(x => x.Id == id);

            var roleRequestDto = new RoleRequestDto()
            {
                Id = id,
                Name = roleResponseDto.Name
            };

            return View(roleRequestDto);
        }

        public IActionResult UserRoles()
        {
            return View();
        }

        [ActionName("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(RoleRequestDto roleRequestDto)
        {
            if (ModelState.IsValid)
            {
                var roleToAdd = _mapper.Map<Role>(roleRequestDto);

                await _roleManager.CreateAsync(roleToAdd);

                return Json(new { isValid = true, message = _localization["Roles.Notification.Add.Text"].Value });
            }

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Add", roleRequestDto) });
        }

        [ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(RoleRequestDto roleRequestDto)
        {
            if (ModelState.IsValid)
            {
                var roleToUpdate = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == roleRequestDto.Id);

                if (roleToUpdate == null)
                {
                    throw new Exception("Güncellenecek rol bulunamamıştır.");
                }

                roleToUpdate.Name = roleRequestDto.Name;

                await _roleManager.UpdateAsync(roleToUpdate);

                return Json(new { isValid = true, message = _localization["Roles.Notification.Edit.Text"].Value });
            }

            return Json(new { isValid = false, html = await _razorViewConverter.GetStringFromRazorView(this, "Edit", roleRequestDto) });
        }

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
