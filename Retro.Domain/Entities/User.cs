namespace Retro.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public ICollection<ConversationParticipant> ConversationParticipants { get; set; } =
        new List<ConversationParticipant>();
}