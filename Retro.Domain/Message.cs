namespace Retro.Domain;


public class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public Guid SenderId { get; set; }
    public User Sender { get; set; }

    public Guid ConversationId { get; set; }
    public Conversation Conversation { get; set; }
}
