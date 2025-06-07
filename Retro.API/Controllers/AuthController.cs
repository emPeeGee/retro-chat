using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Retro.Application.Models;
using Retro.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

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

      return Ok(new { userId = result.Value });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto dto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var result = await _userService.LoginUserAsync(dto);

      if (!result.IsSuccess)
        return Unauthorized(new { message = result.Error });

      return Ok(new { token = result.Value });
    }
    
    
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      return Ok($"You are user {userId}");
    }
  }
}
