namespace Retro.Domain;

public class ConversationParticipant
{
    public Guid ConversationId { get; set; }
    public Guid UserId { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public Conversation Conversation { get; set; } = default!;
    public User User { get; set; } = default!;
}