using Retro.Application.DTOs;

namespace Retro.Application.Interfaces;

public interface IMessageService
{
    Task<Result<MessageResponse>> SendMessageAsync(Guid userId, CreateMessageRequest request);
    Task<Result<List<MessageResponse>>> GetMessagesAsync(Guid userId, Guid conversationId);
    Task<Result<MessageResponse>> EditMessageAsync(Guid userId, EditMessageRequest request);
    Task<Result<MessageResponse>> DeleteMessageAsync(Guid userId, Guid messageId);


    Task<Result<MessageReactionResponse>> AddReactionAsync(Guid userId, CreateMessageReactionRequest request);
    Task<Result<bool>> RemoveReactionAsync(Guid userId, Guid messageId, int emojiReactionId);
    Task<Result<List<MessageReactionResponse>>> GetReactionsAsync(Guid userId, Guid messageId);


    Task<Result<bool>> MarkMessageAsReadAsync(Guid userId, Guid messageId);
}