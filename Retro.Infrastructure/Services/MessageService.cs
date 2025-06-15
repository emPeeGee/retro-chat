using Microsoft.EntityFrameworkCore;
using Retro.Application.DTOs;
using Retro.Application.Interfaces;
using Retro.Domain.Entities;
using Retro.Domain.Enums;

namespace Retro.Infrastructure.Services;

public class MessageService : IMessageService
{
    private readonly AppDbContext _db;

    public MessageService(AppDbContext db)
    {
        _db = db;
    }


    public async Task<Result<MessageResponse>> SendMessageAsync(Guid userId, CreateMessageRequest request)
    {
        var isParticipant = await _db.ConversationParticipants
            .AnyAsync(cp => cp.UserId == userId && cp.ConversationId == request.ConversationId);

        if (!isParticipant)
            return Result<MessageResponse>.Failure("User is not a participant of the conversation.");

        var message = new Message
        {
            Content = request.Content,
            SenderId = userId,
            ConversationId = request.ConversationId,
            SentAt = DateTime.UtcNow
        };

        _db.Messages.Add(message);


        // Add MessageStatus for all other participants as "Sent"
        var participantIds = await _db.ConversationParticipants
            .Where(cp => cp.ConversationId == request.ConversationId && cp.UserId != userId)
            .Select(cp => cp.UserId)
            .ToListAsync();

        var messageStatuses = participantIds.Select(pid => new MessageStatus
        {
            MessageId = message.Id,
            UserId = pid,
            Status = MessageDeliveryStatus.Sent,
            UpdatedAt = DateTime.UtcNow
        }).ToList();

        _db.MessageStatuses.AddRange(messageStatuses);

        await _db.SaveChangesAsync();

        var messageDto = new MessageResponse
        {
            Id = message.Id,
            Content = message.Content,
            OriginalContent = message.OriginalContent,
            SenderId = message.SenderId,
            ConversationId = message.ConversationId,
            SentAt = message.SentAt
        };

        return Result<MessageResponse>.Success(messageDto);
    }

    public async Task<Result<List<MessageResponse>>> GetMessagesAsync(Guid userId, Guid conversationId)
    {
        var isParticipant = await _db.ConversationParticipants
            .AnyAsync(cp => cp.UserId == userId && cp.ConversationId == conversationId);

        if (!isParticipant)
            return Result<List<MessageResponse>>.Failure("Access denied.");

        var messages = await _db.Messages
            .Include(m => m.Reactions)
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();


        var messageIds = messages.Select(m => m.Id).ToList();

        var allStatuses = await _db.MessageStatuses
            .Where(ms => messageIds.Contains(ms.MessageId))
            .ToListAsync();


        // Update statuses to Delivered on fetching it (if not yet Read)
        var messageStatuses = await _db.MessageStatuses
            .Where(ms => ms.UserId == userId &&
                         ms.Status == MessageDeliveryStatus.Sent &&
                         messages.Select(m => m.Id).Contains(ms.MessageId))
            .ToListAsync();

        foreach (var ms in messageStatuses)
        {
            ms.Status = MessageDeliveryStatus.Delivered;
            ms.UpdatedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();

        var messageDtos = messages.Select(m =>
        {
            // var status = messageStatuses
            //     .FirstOrDefault(ms => ms.MessageId == m.Id)?.Status.ToString() ?? "Unknown";


            var statuses = allStatuses
                .Where(ms => ms.MessageId == m.Id)
                .Select(ms => new MessageStatusResponse
                {
                    UserId = ms.UserId,
                    Status = ms.Status.ToString()
                }).ToList();

            return new MessageResponse
            {
                Id = m.Id,
                Content = m.Content,
                OriginalContent = m.OriginalContent,
                SenderId = m.SenderId,
                ConversationId = m.ConversationId,
                SentAt = m.SentAt,
                EditedAt = m.EditedAt,
                IsDeleted = m.IsDeleted,
                Statuses = statuses,
                Reactions = m.Reactions.Select(r => new MessageReactionResponse
                {
                    Id = r.Id,
                    EmojiReactionId = r.EmojiReactionId,
                    UserId = r.UserId,
                    MessageId = r.MessageId,
                    CreatedAt = r.CreatedAt
                }).ToList()
            };
        }).ToList();

        return Result<List<MessageResponse>>.Success(messageDtos);
    }

    public async Task<Result<MessageResponse>> EditMessageAsync(Guid userId, EditMessageRequest request)
    {
        var message = await _db.Messages.FindAsync(request.MessageId);

        if (message == null)
            return Result<MessageResponse>.Failure("Message not found.");

        if (message.SenderId != userId)
            return Result<MessageResponse>.Failure("You are not allowed to edit this message.");

        if ((DateTime.UtcNow - message.SentAt).TotalMinutes > 15)
            return Result<MessageResponse>.Failure("You can only edit a message within 15 minutes of sending.");

        // Preserve original
        message.OriginalContent ??= message.Content;
        message.Content = request.NewContent;
        message.EditedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Result<MessageResponse>.Success(new MessageResponse
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


    public async Task<Result<MessageResponse>> DeleteMessageAsync(Guid userId, Guid messageId)
    {
        var message = await _db.Messages.FindAsync(messageId);

        if (message == null || message.IsDeleted)
            return Result<MessageResponse>.Failure("Message not found.");

        if (message.SenderId != userId)
            return Result<MessageResponse>.Failure("You are not allowed to delete this message.");

        message.IsDeleted = true;
        message.OriginalContent = message.Content;
        message.Content = "This message has been deleted.";
        message.EditedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Result<MessageResponse>.Success(new MessageResponse
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


    public async Task<Result<MessageReactionResponse>> AddReactionAsync(Guid userId,
        CreateMessageReactionRequest request)
    {
        var message = await _db.Messages
            .FirstOrDefaultAsync(m => m.Id == request.MessageId);

        if (message == null)
            return Result<MessageReactionResponse>.Failure("Message not found.");

        var isParticipant = await _db.ConversationParticipants
            .AnyAsync(cp => cp.UserId == userId && cp.ConversationId == message.ConversationId);

        if (!isParticipant)
            return Result<MessageReactionResponse>.Failure("User is not a participant in this conversation.");

        var reactionType = await _db.EmojiReactions.FindAsync(request.ReactionId);
        if (reactionType == null)
            return Result<MessageReactionResponse>.Failure("Invalid reaction type.");


        var existingReaction = await _db.MessageReactions
            .AnyAsync(r =>
                r.UserId == userId && r.MessageId == request.MessageId && r.EmojiReactionId == request.ReactionId);


        if (existingReaction)
            return Result<MessageReactionResponse>.Failure("You have already reacted with this emoji.");

        var reaction = new MessageReaction
        {
            Id = Guid.NewGuid(),
            MessageId = request.MessageId,
            UserId = userId,
            EmojiReactionId = request.ReactionId,
            CreatedAt = DateTime.UtcNow
        };

        _db.MessageReactions.Add(reaction);
        await _db.SaveChangesAsync();

        return Result<MessageReactionResponse>.Success(new MessageReactionResponse
        {
            Id = reaction.Id,
            UserId = userId,
            MessageId = request.MessageId,
            EmojiReactionId = request.ReactionId,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<Result<bool>> RemoveReactionAsync(Guid userId, Guid messageId, int emojiReactionId)
    {
        var reaction = await _db.MessageReactions
            .FirstOrDefaultAsync(r =>
                r.UserId == userId && r.MessageId == messageId && r.EmojiReactionId == emojiReactionId);

        if (reaction == null)
            return Result<bool>.Failure("Reaction not found.");

        _db.MessageReactions.Remove(reaction);
        await _db.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    public async Task<Result<List<MessageReactionResponse>>> GetReactionsAsync(Guid userId, Guid messageId)
    {
        var message = await _db.Messages
            .FirstOrDefaultAsync(m => m.Id == messageId);

        if (message == null)
            return Result<List<MessageReactionResponse>>.Failure("Message not found.");

        var isParticipant = await _db.ConversationParticipants
            .AnyAsync(cp => cp.UserId == userId && cp.ConversationId == message.ConversationId);

        if (!isParticipant)
            return Result<List<MessageReactionResponse>>.Failure("User is not a participant in this conversation.");

        var reactions = await _db.MessageReactions
            .Where(r => r.MessageId == messageId)
            .Select(r => new MessageReactionResponse
            {
                Id = r.Id,
                UserId = r.UserId,
                MessageId = r.MessageId,
                EmojiReactionId = r.EmojiReaction.Id,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync();

        return Result<List<MessageReactionResponse>>.Success(reactions);
    }


    public async Task<Result<bool>> MarkMessageAsReadAsync(Guid userId, Guid messageId)
    {
        var messageStatus = await _db.MessageStatuses
            .FirstOrDefaultAsync(ms => ms.UserId == userId && ms.MessageId == messageId);

        if (messageStatus == null)
            return Result<bool>.Failure("Message status not found.");

        if (messageStatus.Status != MessageDeliveryStatus.Read)
        {
            messageStatus.Status = MessageDeliveryStatus.Read;
            messageStatus.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        return Result<bool>.Success(true);
    }
}