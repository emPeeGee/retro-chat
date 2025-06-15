using Retro.Domain.Enums;

namespace Retro.Domain.Entities;

public class MessageStatus
{
    public Guid MessageId { get; set; }
    public Message Message { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public MessageDeliveryStatus Status { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}