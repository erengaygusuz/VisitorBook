using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Utilities.DataTablesServerSideHelpers;
using VisitorBook.Backend.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Backend.Core.Dtos.UserDtos;

namespace VisitorBook.Backend.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager, IMapper mapper) : base(mapper)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("GetTableData")]
        public IActionResult GetAllUsers([FromBody] DataTablesOptions dataTablesOptions)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            return DataTablesResult(_userManager.Users.Select(x => new UserResponseDto { }).ToPagedList(dataTablesOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            var userResponseDtos = _mapper.Map<List<UserResponseDto>>(users);

            return Ok(userResponseDtos);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var userResponseDto = _mapper.Map<UserResponseDto>(user);

            return Ok(userResponseDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userToDelete = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (userToDelete == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(userToDelete);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDto userRequestDto)
        {
            if (userRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var userToAdd = _mapper.Map<User>(userRequestDto);

            await _userManager.CreateAsync(userToAdd);

            return CreatedAtRoute("GetUser", userToAdd, new { id = userToAdd.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequestDto userRequestDto)
        {
            var userToUpdate = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            if (userRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            _mapper.Map(userRequestDto, userToUpdate);

            await _userManager.UpdateAsync(userToUpdate);

            return NoContent();
        }
    }
}
