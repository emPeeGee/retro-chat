namespace Retro.Application.DTOs;

public class EditMessageRequest
{
    public Guid MessageId { get; set; }
    public string NewContent { get; set; }
}
