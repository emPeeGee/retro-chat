using Microsoft.AspNetCore.Mvc;
using Retro.Application.Models;
using Retro.Application.Interfaces;

namespace Retro.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto dto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var result = await _userService.RegisterUserAsync(dto);

      if (!result.IsSuccess)
        return BadRequest(new { message = result.Error });

      return Ok(new { message = "User registered successfully." });
    }
  }
}
