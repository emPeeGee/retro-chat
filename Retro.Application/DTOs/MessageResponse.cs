namespace Retro.Application.DTOs;

public class MessageResponse
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public string? OriginalContent { get; set; }
    public Guid SenderId { get; set; }
    public Guid ConversationId { get; set; }
    public DateTime SentAt { get; set; }
    public DateTime? EditedAt { get; set; }
    public bool IsDeleted { get; set; }
    public List<MessageReactionResponse>? Reactions { get; set; }
    public string Status { get; set; } = string.Empty;
}