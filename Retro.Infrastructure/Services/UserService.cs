using Microsoft.EntityFrameworkCore;
using Retro.Application.DTOs;
using Retro.Application.Interfaces;
using Retro.Application.Models;
using Retro.Domain.Entities;

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

    public async Task<Result<Guid>> RegisterUserAsync(UserRegisterRequest request)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == request.Email);
        if (exists)
            return Result<Guid>.Failure("Email is already registered.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Username = request.Username,
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwordService.HashPassword(request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Result<Guid>.Success(user.Id);
    }

    public async Task<Result<string>> LoginUserAsync(UserLoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user is null)
            return Result<string>.Failure("Invalid credentials");

        var result = _passwordService.VerifyPassword(user.PasswordHash, request.Password);
        if (!result)
            return Result<string>.Failure("Invalid credentials");

        var jwt = _tokenService.GenerateJwtToken(user.Id, user.Email);

        return Result<string>.Success(jwt);
    }

    public async Task<UserResponse?> GetCurrentUserAsync(Guid userId)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return null;

        return new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}