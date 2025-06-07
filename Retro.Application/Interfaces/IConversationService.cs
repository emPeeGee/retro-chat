using Retro.Application.DTOs;

namespace Retro.Application.Interfaces;

public interface IConversationService
{
    Task<Result<CreateConversationResponse>> CreateConversationAsync(Guid currentUserId, CreateConversationRequest request);
    Task<Result<List<ConversationResponse>>> GetConversationsForUserAsync(Guid userId);
    Task<Result<ConversationResponse>> GetConversationByIdAsync(Guid userId, Guid conversationId);
}
