using Retro.Application.DTOs;

namespace Retro.Application.Interfaces;

public interface IMessageService
{
    Task<Result<MessageDto>> SendMessageAsync(Guid userId, CreateMessageRequest request);
    Task<Result<List<MessageDto>>> GetMessagesAsync(Guid userId, Guid conversationId);
    Task<Result<MessageDto>> EditMessageAsync(Guid userId, EditMessageRequest request);
    Task<Result<MessageDto>> DeleteMessageAsync(Guid userId, Guid messageId);
    
    
    Task<Result<MessageReactionDto>> AddReactionAsync(Guid userId, CreateMessageReactionRequest request);
    Task<Result<List<MessageReactionDto>>> GetReactionsAsync(Guid userId, Guid messageId);
    
}