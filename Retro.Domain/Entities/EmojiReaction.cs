namespace Retro.Domain.Entities;

public class EmojiReaction
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Emoji { get; set; }

    public ICollection<MessageReaction> MessageReactions { get; set; } = new List<MessageReaction>();
}