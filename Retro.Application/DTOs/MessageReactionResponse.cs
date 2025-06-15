namespace Retro.Application.DTOs;

public class MessageReactionResponse
{
    public Guid Id { get; set; }
    public Guid MessageId { get; set; }
    public Guid UserId { get; set; }
    public int EmojiReactionId { get; set; }
    public DateTime CreatedAt { get; set; }
}