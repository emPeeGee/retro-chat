namespace Retro.Application.DTOs;

public class CreateMessageRequest
{
    public string Content { get; set; } = string.Empty;
    public Guid ConversationId { get; set; }
}