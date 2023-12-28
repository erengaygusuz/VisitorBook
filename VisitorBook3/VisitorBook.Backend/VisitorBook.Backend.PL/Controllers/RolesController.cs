using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Utilities.DataTablesServerSideHelpers;
using VisitorBook.Backend.Core.Extensions;
using VisitorBook.Backend.Core.Dtos.RoleDtos;
using VisitorBook.Backend.Core.Dtos.UserRoleDtos;

namespace VisitorBook.Backend.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public RolesController(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper) : base(mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("GetTableData")]
        public IActionResult GetAllRoles([FromBody] DataTablesOptions dataTablesOptions)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            return DataTablesResult(_roleManager.Roles.Select(x => new RoleResponseDto { }).ToPagedList(dataTablesOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            if (roles == null)
            {
                return NotFound();
            }

            var roleResponseDtos = _mapper.Map<List<RoleResponseDto>>(roles);

            return Ok(roleResponseDtos);
        }

        [HttpGet("{id}", Name = "GetRole")]
        public async Task<IActionResult> GetRole(int id)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (role == null)
            {
                return NotFound();
            }

            var roleResponseDto = _mapper.Map<RoleResponseDto>(role);

            return Ok(roleResponseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var roleToDelete = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (roleToDelete == null)
            {
                return NotFound();
            }

            await _roleManager.DeleteAsync(roleToDelete);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequestDto roleRequestDto)
        {
            if (roleRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var roleToAdd = _mapper.Map<Role>(roleRequestDto);

            await _roleManager.CreateAsync(roleToAdd);

            return CreatedAtRoute("GetRole", roleToAdd, new { id = roleToAdd.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleRequestDto roleRequestDto)
        {
            var roleToUpdate = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (roleToUpdate == null)
            {
                return NotFound();
            }

            if (roleRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            _mapper.Map(roleRequestDto, roleToUpdate);

            await _roleManager.UpdateAsync(roleToUpdate);

            return NoContent();
        }

        [HttpPost]
        [Route("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser(string userId, [FromBody] UserRoleRequestDto userRoleRequestDto)
        {
            var userToAssignRoles = await _userManager.FindByIdAsync(userId);

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

            return NoContent();
        }
    }
}
