using Retro.Application.DTOs;
using Retro.Application.Models;

namespace Retro.Application.Interfaces;

public interface IUserService
{
    Task<Result<Guid>> RegisterUserAsync(UserRegisterRequest request);
    Task<Result<string>> LoginUserAsync(UserLoginRequest request);
    Task<UserResponse?> GetCurrentUserAsync(Guid userId);
}