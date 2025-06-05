using Retro.Application.Models;
using System.Threading.Tasks;

namespace Retro.Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<Guid>> RegisterUserAsync(UserRegisterDto dto);
        Task<Result<string>> LoginUserAsync(UserLoginDto dto);
    }
}
