namespace Retro.Application.DTOs;

public class CreateMessageReactionRequest
{
    public Guid MessageId { get; set; }
    public int ReactionId { get; set; } 
}