using Retro.Application.Models;
using System.Threading.Tasks;

namespace Retro.Application.Interfaces
{
    public interface IUserService
    {
        Task<Guid> RegisterUserAsync(UserRegisterDto dto);
    }
}
