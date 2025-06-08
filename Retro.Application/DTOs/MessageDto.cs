namespace Retro.Application.DTOs;


public class MessageDto
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public Guid SenderId { get; set; }
    public Guid ConversationId { get; set; }
}