namespace Retro.Application.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(Guid userId, string email);
}