using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retro.Application.DTOs;
using Retro.Application.Helpers;
using Retro.Application.Interfaces;

namespace Retro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserResponse>> GetMe()
    {
        var userId = User.GetUserId();

        var user = await _userService.GetCurrentUserAsync(userId);

        if (user == null)
            return NotFound();

        return Ok(user);
    }
}