namespace Retro.Domain;

public class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Content { get; set; } = string.Empty;

    public string? OriginalContent { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public DateTime? EditedAt { get; set; }
    public Guid SenderId { get; set; }
    public Guid ConversationId { get; set; }

    public User Sender { get; set; }
    public Conversation Conversation { get; set; }
    public ICollection<MessageReaction> Reactions { get; set; } = new List<MessageReaction>();
}