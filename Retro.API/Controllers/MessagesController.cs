using Retro.Application.DTOs;
using Retro.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retro.Application.Helpers;

namespace Retro.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] CreateMessageRequest request)
    {
        var userId = User.GetUserId(); 
        var result = await _messageService.SendMessageAsync(userId, request);

        if (result.IsSuccess == false)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("{conversationId:guid}")]
    public async Task<IActionResult> GetMessages(Guid conversationId)
    {
        var userId = User.GetUserId(); 

        var result = await _messageService.GetMessagesAsync(userId, conversationId);

        if (result.IsSuccess == false)
            return Forbid();

        return Ok(result.Value);
    }
    
    
    [Authorize]
    [HttpPut("edit")]
    public async Task<IActionResult> EditMessage([FromBody] EditMessageRequest request)
    {
        var userId = User.GetUserId();

        var result = await _messageService.EditMessageAsync(userId, request);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMessage(Guid id)
    {
        var userId = User.GetUserId();

        var result = await _messageService.DeleteMessageAsync(userId, id);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
