using Microsoft.AspNetCore.Mvc;
using Oliva.Models;
using Oliva.Services;

namespace Oliva.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var usersList = await _userService.GetAllUsersAsync();
            return Ok(usersList);
        }

        [HttpGet("{UUID}")]
        public async Task<IActionResult> GetUserByUUID(string UUID)
        {
            var dbUser = await _userService.GetUserByUUIDAsync(UUID);

            if (dbUser == null)
            {
                return NotFound("User not found.");
            }

            return Ok(dbUser);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdUser = await _userService.CreateNewUserAsync(userDto);
                
                return CreatedAtAction(nameof(GetUserByUUID), new { UUID = createdUser.UUID }, createdUser);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }

        [HttpPut("{UUID}")]
        public async Task<IActionResult> UpdateUser(string UUID, [FromBody] UpdateUserDto userDto)
        {
            try
            {
                await _userService.UpdateUserAsync(UUID, userDto);
                return Ok("User updated with success.");
            }
            catch (Exception error)
            {
                return NotFound(error.Message);
            }
        }

        [HttpDelete("{UUID}")]
        public async Task<IActionResult> DeleteUser(string UUID)
        {
            try
            {
                await _userService.DeleteUserAsync(UUID);
                return Ok("User deleted with success.");
            }
            catch (Exception error)
            {
                return NotFound(error.Message);
            }
        }
    }
}