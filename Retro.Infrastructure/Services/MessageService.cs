using Microsoft.EntityFrameworkCore;
using Retro.Application.DTOs;
using Retro.Application.Interfaces;
using Retro.Domain;

namespace Retro.Infrastructure.Services;

public class MessageService : IMessageService
{
    private readonly AppDbContext _db;

    public MessageService(AppDbContext db)
    {
        _db = db;
    }


    public async Task<Result<MessageDto>> SendMessageAsync(Guid userId, CreateMessageRequest request)
    {
        var isParticipant = await _db.ConversationParticipants
            .AnyAsync(cp => cp.UserId == userId && cp.ConversationId == request.ConversationId);

        if (!isParticipant)
            return Result<MessageDto>.Failure("User is not a participant of the conversation.");

        var message = new Message
        {
            Content = request.Content,
            SenderId = userId,
            ConversationId = request.ConversationId,
            SentAt = DateTime.UtcNow
        };

        _db.Messages.Add(message);
        await _db.SaveChangesAsync();

        var messageDto = new MessageDto
        {
            Id = message.Id,
            Content = message.Content,
            OriginalContent = message.OriginalContent,
            SenderId = message.SenderId,
            ConversationId = message.ConversationId,
            SentAt = message.SentAt
        };

        return Result<MessageDto>.Success(messageDto);
    }

    public async Task<Result<List<MessageDto>>> GetMessagesAsync(Guid userId, Guid conversationId)
    {
        var isParticipant = await _db.ConversationParticipants
            .AnyAsync(cp => cp.UserId == userId && cp.ConversationId == conversationId);

        if (!isParticipant)
            return Result<List<MessageDto>>.Failure("Access denied.");

        var messages = await _db.Messages
            .Include(m => m.Reactions)
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        var messageDtos = messages.Select(m => new MessageDto
        {
            Id = m.Id,
            Content = m.Content,
            OriginalContent = m.OriginalContent,
            SenderId = m.SenderId,
            ConversationId = m.ConversationId,
            SentAt = m.SentAt,
            EditedAt = m.EditedAt,
            IsDeleted = m.IsDeleted,
            Reactions = m.Reactions.Select(r => new MessageReactionDto
            {
                Id = r.Id,
                EmojiReactionId = r.EmojiReactionId,
                UserId = r.UserId,
                MessageId = r.MessageId,
                CreatedAt = r.CreatedAt
            }).ToList()
        }).ToList();

        return Result<List<MessageDto>>.Success(messageDtos);
    }

    public async Task<Result<MessageDto>> EditMessageAsync(Guid userId, EditMessageRequest request)
    {
        var message = await _db.Messages.FindAsync(request.MessageId);

        if (message == null)
            return Result<MessageDto>.Failure("Message not found.");

        if (message.SenderId != userId)
            return Result<MessageDto>.Failure("You are not allowed to edit this message.");

        if ((DateTime.UtcNow - message.SentAt).TotalMinutes > 15)
            return Result<MessageDto>.Failure("You can only edit a message within 15 minutes of sending.");

        // Preserve original
        message.OriginalContent ??= message.Content;
        message.Content = request.NewContent;
        message.EditedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Result<MessageDto>.Success(new MessageDto
        {
            Id = message.Id,
            Content = message.Content,
            SenderId = message.SenderId,
            SentAt = message.SentAt,
            EditedAt = message.EditedAt,
            IsDeleted = message.IsDeleted,
            OriginalContent = message.OriginalContent,
            ConversationId = message.ConversationId
        });
    }


    public async Task<Result<MessageDto>> DeleteMessageAsync(Guid userId, Guid messageId)
    {
        var message = await _db.Messages.FindAsync(messageId);

        if (message == null || message.IsDeleted)
            return Result<MessageDto>.Failure("Message not found.");

        if (message.SenderId != userId)
            return Result<MessageDto>.Failure("You are not allowed to delete this message.");

        message.IsDeleted = true;
        message.OriginalContent = message.Content;
        message.Content = "This message has been deleted.";
        message.EditedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Result<MessageDto>.Success(new MessageDto
        {
            Id = message.Id,
            Content = message.Content,
            SenderId = message.SenderId,
            SentAt = message.SentAt,
            EditedAt = message.EditedAt,
            IsDeleted = message.IsDeleted,
            OriginalContent = message.OriginalContent
        });
    }


    public async Task<Result<MessageReactionDto>> AddReactionAsync(Guid userId, CreateMessageReactionRequest request)
    {
        var message = await _db.Messages
            .FirstOrDefaultAsync(m => m.Id == request.MessageId);

        if (message == null)
            return Result<MessageReactionDto>.Failure("Message not found.");

        var isParticipant = await _db.ConversationParticipants
            .AnyAsync(cp => cp.UserId == userId && cp.ConversationId == message.ConversationId);

        if (!isParticipant)
            return Result<MessageReactionDto>.Failure("User is not a participant in this conversation.");

        var reactionType = await _db.EmojiReactions.FindAsync(request.ReactionId);
        if (reactionType == null)
            return Result<MessageReactionDto>.Failure("Invalid reaction type.");

        var existingReaction = await _db.MessageReactions
            .FirstOrDefaultAsync(r => r.MessageId == request.MessageId && r.UserId == userId);

        if (existingReaction != null)
        {
            existingReaction.EmojiReactionId = request.ReactionId; // User changes reaction
        }
        else
        {
            var reaction = new MessageReaction
            {
                Id = Guid.NewGuid(),
                MessageId = request.MessageId,
                UserId = userId,
                EmojiReactionId = request.ReactionId,
                CreatedAt = DateTime.UtcNow
            };
            _db.MessageReactions.Add(reaction);
        }

        await _db.SaveChangesAsync();
        return Result<MessageReactionDto>.Success(new MessageReactionDto
        {
            Id = existingReaction?.Id ?? Guid.NewGuid(),
            UserId = userId,
            MessageId = request.MessageId,
            EmojiReactionId = request.ReactionId,
            CreatedAt = DateTime.UtcNow
        });
    }


    public async Task<Result<List<MessageReactionDto>>> GetReactionsAsync(Guid userId, Guid messageId)
    {
        var message = await _db.Messages
            .FirstOrDefaultAsync(m => m.Id == messageId);

        if (message == null)
            return Result<List<MessageReactionDto>>.Failure("Message not found.");

        var isParticipant = await _db.ConversationParticipants
            .AnyAsync(cp => cp.UserId == userId && cp.ConversationId == message.ConversationId);

        if (!isParticipant)
            return Result<List<MessageReactionDto>>.Failure("User is not a participant in this conversation.");

        var reactions = await _db.MessageReactions
            .Where(r => r.MessageId == messageId)
            .Select(r => new MessageReactionDto
            {
                Id = r.Id,
                UserId = r.UserId,
                MessageId = r.MessageId,
                EmojiReactionId = r.EmojiReaction.Id,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();

        return Result<List<MessageReactionDto>>.Success(reactions);
    }
}