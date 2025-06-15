using Microsoft.EntityFrameworkCore;
using Retro.Application.Interfaces;
using Retro.Application.Models;
using Retro.Domain;

namespace Retro.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public UserService(AppDbContext context, IPasswordService passwordService, ITokenService tokenService)
    {
        _context = context;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<Result<Guid>> RegisterUserAsync(UserRegisterDto dto)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (exists)
            return Result<Guid>.Failure("Email is already registered.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            Username = dto.Username,
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwordService.HashPassword(dto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Result<Guid>.Success(user.Id);
    }

    public async Task<Result<string>> LoginUserAsync(UserLoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user is null)
            return Result<string>.Failure("Invalid credentials");

        var result = _passwordService.VerifyPassword(user.PasswordHash, dto.Password);
        if (!result)
            return Result<string>.Failure("Invalid credentials");

        var jwt = _tokenService.GenerateJwtToken(user.Id, user.Email);

        return Result<string>.Success(jwt);
    }
}