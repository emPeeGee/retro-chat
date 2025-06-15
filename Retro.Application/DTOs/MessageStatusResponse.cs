namespace Retro.Application.DTOs;

public class MessageStatusResponse
{
    public Guid UserId { get; set; }
    public string Status { get; set; } = string.Empty;
}