using Retro.Application.DTOs;

namespace Retro.Application.Interfaces;

public interface IMessageService
{
    Task<Result<MessageDto>> SendMessageAsync(Guid userId, CreateMessageRequest request);
    Task<Result<List<MessageDto>>> GetMessagesAsync(Guid userId, Guid conversationId);
}