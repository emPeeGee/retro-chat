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
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        var messageDtos = messages.Select(m => new MessageDto
        {
            Id = m.Id,
            Content = m.Content,
            SenderId = m.SenderId,
            ConversationId = m.ConversationId,
            SentAt = m.SentAt
        }).ToList();

        return Result<List<MessageDto>>.Success(messageDtos);
    }
}
