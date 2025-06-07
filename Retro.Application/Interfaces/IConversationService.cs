using Retro.Application.DTOs;

namespace Retro.Application.Interfaces;

public interface IConversationService
{
    Task<Result<ConversationResponse>> CreateConversationAsync(Guid currentUserId, CreateConversationRequest request);
}
