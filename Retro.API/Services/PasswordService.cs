using Microsoft.AspNetCore.Identity;
using Retro.Application.Interfaces;
using Retro.Domain;

namespace Retro.API.Services;

public class PasswordService : IPasswordService
{
  private readonly PasswordHasher<User> _hasher = new();

  public string HashPassword(string password)
  {
    return _hasher.HashPassword(null!, password);
  }

  public bool VerifyPassword(string hashedPassword, string providedPassword)
  {
    var result = _hasher.VerifyHashedPassword(null!, hashedPassword, providedPassword);
    return result == PasswordVerificationResult.Success;
  }
}
