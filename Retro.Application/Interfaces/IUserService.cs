using Retro.Application.Models;
using System.Threading.Tasks;

namespace Retro.Application.Interfaces
{
    public interface IUserService
    {
        Task<Result> RegisterUserAsync(UserRegisterDto dto);
    }
}
