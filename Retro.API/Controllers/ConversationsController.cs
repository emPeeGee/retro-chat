using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retro.Application.DTOs;
using Retro.Application.Interfaces;
using System.Security.Claims;
using Retro.Application.Helpers;

namespace Retro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConversationsController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public ConversationsController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateConversationRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _conversationService.CreateConversationAsync(userId, request);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(Create), new { id = result.Value.Id }, result.Value);
    }
    
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var userId = User.GetUserId(); 
        var result = await _conversationService.GetConversationsForUserAsync(userId);

        if (result.IsSuccess == false)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
