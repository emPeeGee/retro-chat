using Microsoft.EntityFrameworkCore;
using Retro.Application.DTOs;
using Retro.Application.Interfaces;
using Retro.Domain;

namespace Retro.Infrastructure.Services;

public class ConversationService : IConversationService
{
    private readonly AppDbContext _db;

    public ConversationService(AppDbContext db)
    {
        _db = db;
    }


    public async Task<Result<CreateConversationResponse>> CreateConversationAsync(Guid currentUserId,
        CreateConversationRequest request)
    {
        // Ensure at least 1 other participant
        if (request.ParticipantUserIds == null || !request.ParticipantUserIds.Any())
            return Result<CreateConversationResponse>.Failure("At least one participant is required.");

        // Prevent duplicate direct chat
        if (!request.IsGroup && request.ParticipantUserIds.Count == 1)
        {
            var otherUserId = request.ParticipantUserIds.First();

            var existing = await _db.Conversations
                .Where(c => !c.IsGroup)
                .Include(c => c.Participants)
                .FirstOrDefaultAsync(c =>
                    c.Participants.Any(p => p.UserId == currentUserId) &&
                    c.Participants.Any(p => p.UserId == otherUserId) &&
                    c.Participants.Count == 2);

            if (existing != null)
                return Result<CreateConversationResponse>.Failure("Direct conversation already exists.");
        }

        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            IsGroup = request.IsGroup,
            Name = request.IsGroup ? request.Title : null,
            CreatedAt = DateTime.UtcNow,
            Participants = request.ParticipantUserIds
                .Append(currentUserId)
                .Distinct()
                .Select(uid => new ConversationParticipant
                {
                    UserId = uid
                })
                .ToList()
        };

        _db.Conversations.Add(conversation);
        await _db.SaveChangesAsync();

        return Result<CreateConversationResponse>.Success(new CreateConversationResponse
        {
            Id = conversation.Id,
            Title = conversation.Name,
            IsGroup = conversation.IsGroup,
            ParticipantUserIds = conversation.Participants.Select(p => p.UserId).ToList()
        });
    }

    public async Task<Result<List<ConversationResponse>>> GetConversationsForUserAsync(Guid userId)
    {
        var conversations = await _db.ConversationParticipants
            .Where(cp => cp.UserId == userId)
            .Include(cp => cp.Conversation)
            .ThenInclude(c => c.Participants)
            .ThenInclude(cp => cp.User)
            .Select(cp => cp.Conversation)
            .Distinct()
            .ToListAsync();

        var response = conversations.Select(c => new ConversationResponse
        {
            Id = c.Id,
            Title = c.Name,
            Participants = c.Participants.Select(p => new ParticipantDto
            {
                Id = p.User.Id,
                Email = p.User.Email
            }).ToList()
        }).ToList();

        return Result<List<ConversationResponse>>.Success(response);
    }

    public async Task<Result<ConversationResponse>> GetConversationByIdAsync(Guid userId, Guid conversationId)
    {
        var conversation = await _db.Conversations
            .Include(c => c.Participants)
            .ThenInclude(cp => cp.User)
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation == null)
            return Result<ConversationResponse>.Failure("Conversation not found.");

        var isParticipant = conversation.Participants.Any(p => p.UserId == userId);
        if (!isParticipant)
            return Result<ConversationResponse>.Failure("Access deniedd.");

        var response = new ConversationResponse
        {
            Id = conversation.Id,
            Title = conversation.Name,
            Participants = conversation.Participants.Select(p => new ParticipantDto
            {
                Id = p.User.Id,
                Email = p.User.Email
            }).ToList()
        };

        return Result<ConversationResponse>.Success(response);
    }
}