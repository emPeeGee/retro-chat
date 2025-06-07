namespace Retro.Application.DTOs;

public class ConversationResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public List<ParticipantDto> Participants { get; set; }
}