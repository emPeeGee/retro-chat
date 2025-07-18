namespace Retro.Domain.Entities;

public class MessageReaction
{
    public Guid Id { get; set; } // PK
    public Guid MessageId { get; set; }
    public Guid UserId { get; set; }
    public int EmojiReactionId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Message Message { get; set; }
    public User User { get; set; }
    public EmojiReaction EmojiReaction { get; set; }
}