namespace Retro.Application.DTOs;

public class CreateConversationRequest
{
    public string? Title { get; set; } 
    public bool IsGroup { get; set; }  // True = group, False = direct
    public List<Guid> ParticipantUserIds { get; set; } = [];
}