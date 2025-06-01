using Microsoft.AspNetCore.Mvc;
using Retro.Application.Models;
using Retro.Application.Interfaces;

namespace Retro.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserRegisterDto dto)
        {
            try
            {
                var userId = await _userService.RegisterUserAsync(dto);
                return CreatedAtAction(nameof(RegisterUser), new { id = userId }, new { id = userId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
