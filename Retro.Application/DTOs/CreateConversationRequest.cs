namespace Retro.Application.DTOs;

public class CreateConversationRequest
{
    public string? Title { get; set; }
    public bool IsGroup { get; set; } //  False = direct
    public List<Guid> ParticipantUserIds { get; set; } = [];
}