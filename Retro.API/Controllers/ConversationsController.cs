using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retro.Application.DTOs;
using Retro.Application.Interfaces;
using System.Security.Claims;

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
    public async Task<IActionResult> Create([FromBody] CreateConversationRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _conversationService.CreateConversationAsync(userId, request);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(Create), new { id = result.Value.Id }, result.Value);
    }
}
