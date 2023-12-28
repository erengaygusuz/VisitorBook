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
using VisitorBook.Core.Dtos.UserRoleDtos;

namespace VisitorBook.UI.Areas.Admin.Controllers
{
    [Area("App")]
    public class RoleController : BaseController
    {
        private readonly IStringLocalizer<Language> _localization;
        private readonly RazorViewConverter _razorViewConverter;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly RoleDataTablesOptions _roleDataTableOptions;
        private readonly IMapper _mapper;

        public RoleController(RazorViewConverter razorViewConverter,
            IStringLocalizer<Language> localization, UserManager<User> userManager, RoleManager<Role> roleManager, RoleDataTablesOptions roleDataTableOptions,
            IMapper mapper)
        {
            _userManager = userManager;
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
                Name = roleResponseDto.Name
            };

            return View(roleRequestDto);
        }

        [NoDirectAccess]
        public async Task<IActionResult> AssignRoleToUser(string id)
        {
            var currentUser = await _userManager.FindByIdAsync(id);

            ViewBag.userId = id;

            var roles = await _roleManager.Roles.ToListAsync();

            var userRoleResponseDtos = new List<UserRoleResponseDto>();

            var userRoles = await _userManager.GetRolesAsync(currentUser);

            foreach (var role in roles)
            {
                var userRoleResponseDto = new UserRoleResponseDto()
                {
                    Id = role.Id,
                    Name = role.Name
                };

                if (userRoles.Contains(role.Name))
                {
                    userRoleResponseDto.Exist = true;
                }

                userRoleResponseDtos.Add(userRoleResponseDto);
            }

            return View(userRoleResponseDtos);
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

                return Json(new { isValid = true, message = _localization["Regions.Notification.Add.Text"].Value });
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
                var roleToUpdate = _mapper.Map<Role>(roleRequestDto);

                await _roleManager.UpdateAsync(roleToUpdate);

                return Json(new { isValid = true, message = _localization["Regions.Notification.Edit.Text"].Value });
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

        [ActionName("AssignRoleToUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRoleToUser(UserRoleRequestDto userRoleRequestDto)
        {
            var userToAssignRoles = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userRoleRequestDto.UserId);

            if (userToAssignRoles == null)
            {
                return NotFound();
            }

            if (userRoleRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            foreach (var userRoleInfo in userRoleRequestDto.UserRoleInfo)
            {
                if (userRoleInfo.Exist)
                {
                    await _userManager.AddToRoleAsync(userToAssignRoles, userRoleInfo.Name);
                }

                else
                {
                    await _userManager.RemoveFromRoleAsync(userToAssignRoles, userRoleInfo.Name);
                }
            }

            return RedirectToAction(nameof(UserController.Index), "User");
        }
    }
}
