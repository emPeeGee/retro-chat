namespace Retro.Application.DTOs;


public class CreateConversationResponse
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public bool IsGroup { get; set; }
    public List<Guid> ParticipantUserIds { get; set; } = [];
}
