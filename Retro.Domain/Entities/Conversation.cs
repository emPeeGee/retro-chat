namespace Retro.Domain.Entities;

public class Conversation
{
    public Guid Id { get; set; }
    public string? Name { get; set; } // Optional for private, used for group
    public bool IsGroup { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ConversationParticipant> Participants { get; set; } = new List<ConversationParticipant>();
}