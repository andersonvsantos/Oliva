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
            var usersList = await _userService.GetAllAsync();
            return Ok(usersList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var foundUser = await _userService.GetUserByIdAsync(id);

            if (foundUser == null)
            {
                return NotFound("User not found.");
            }

            return Ok(foundUser);
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
                
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception erro)
            {
                return BadRequest(erro.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto userDto)
        {
            try
            {
                await _userService.UpdateUserAsync(id, userDto);
                return NoContent(); 
            }
            catch (Exception erro)
            {
                return NotFound(erro.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok("User deleted with success.");
            }
            catch (Exception erro)
            {
                return NotFound(erro.Message);
            }
        }
    }
}