
using Microsoft.EntityFrameworkCore;
using Retro.Application.DTOs;
using Retro.Application.Interfaces;
using Retro.Domain;

namespace Retro.Infrastructure.Services
{
    public class ConversationService : IConversationService
    {
        private readonly AppDbContext _db;

        public ConversationService(AppDbContext db)
        {
            _db = db;
        }
        
        
        public async Task<Result<ConversationResponse>> CreateConversationAsync(Guid currentUserId, CreateConversationRequest request)
        {
            // Ensure at least 1 other participant
            if (request.ParticipantUserIds == null || !request.ParticipantUserIds.Any())
                return Result<ConversationResponse>.Failure("At least one participant is required.");

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
                {
                    return Result<ConversationResponse>.Failure("Direct conversation already exists.");
                }
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

            return Result<ConversationResponse>.Success(new ConversationResponse
            {
                Id = conversation.Id,
                Title = conversation.Name,
                IsGroup = conversation.IsGroup,
                ParticipantUserIds = conversation.Participants.Select(p => p.UserId).ToList()
            });
        }

        public async Task<Result<Conversation>> GetConversationByIdAsync(Guid conversationId)
        {
            var conversation = await _db.Conversations
                .Include(c => c.Participants)
                .FirstOrDefaultAsync(c => c.Id == conversationId);

            if (conversation == null)
                return Result<Conversation>.Failure("Conversation not found");

            return Result<Conversation>.Success(conversation);
        }

        public async Task<Result<List<Conversation>>> GetUserConversationsAsync(Guid userId)
        {
            var conversations = await _db.ConversationParticipants
                .Include(cp => cp.Conversation)
                .Where(cp => cp.UserId == userId)
                .Select(cp => cp.Conversation)
                .ToListAsync();

            return Result<List<Conversation>>.Success(conversations);
        }

    }
}
