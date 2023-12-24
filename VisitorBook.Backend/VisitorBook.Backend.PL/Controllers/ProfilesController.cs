using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Backend.Core.Dtos.ProfileDtos;
using VisitorBook.Backend.Core.Dtos.UserDtos;
using VisitorBook.Backend.Core.Entities;
using VisitorBook.Backend.Core.Extensions;

namespace VisitorBook.Backend.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public ProfilesController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange([FromBody] PasswordChangeRequestDto passwordChangeRequestDto)
        {
            if (passwordChangeRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (currentUser == null)
            {
                return NotFound();
            }

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, passwordChangeRequestDto.PasswordOld);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");

                return BadRequest();
            }

            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, passwordChangeRequestDto.PasswordOld, passwordChangeRequestDto.PasswordNew);

            if (!resultChangePassword.Succeeded)
            {
                ModelState.AddModelErrorList(resultChangePassword.Errors.Select(x => x.Description).ToList());

                return BadRequest();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser);

            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, passwordChangeRequestDto.PasswordNew, true, false);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGeneralInfo(int id, [FromBody] UserRequestDto userRequestDto)
        {
            if (userRequestDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState.GetValidationErrors());
            }

            var userToUpdate = await _userManager.Users.FirstAsync(x => x.Id == id);

            if (userToUpdate == null)
            {
                return NotFound();
            }

            userToUpdate.Name = userRequestDto.Name;
            userToUpdate.Surname = userRequestDto.Surname;
            userToUpdate.BirthDate = userRequestDto.BirthDate;
            userToUpdate.Gender = userRequestDto.Gender;

            _mapper.Map(userRequestDto, userToUpdate);

            var updateToUserResult = await _userManager.UpdateAsync(userToUpdate);

            if (!updateToUserResult.Succeeded)
            {
                ModelState.AddModelErrorList(updateToUserResult.Errors);

                return BadRequest();
            }

            await _userManager.UpdateSecurityStampAsync(userToUpdate);

            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(userToUpdate, true);

            return NoContent();
        }
    }
}
