using Retro.Application.Models;

namespace Retro.Application.Interfaces;

public interface IUserService
{
    Task<Result<Guid>> RegisterUserAsync(UserRegisterDto dto);
    Task<Result<string>> LoginUserAsync(UserLoginDto dto);
}