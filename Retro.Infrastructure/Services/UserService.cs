using Retro.Application.Models;
using Retro.Application.Interfaces;
using Retro.Domain;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;


namespace Retro.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;


        public UserService(AppDbContext context, IPasswordService passwordService, IConfiguration config, ITokenService tokenService)
        {
            _context = context;
            _passwordService = passwordService;
            _config = config;
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

        // public async Task<Result> RegisterUserAsync(UserRegisterDto dto)
        // {
        //     if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
        //         return Result.Failure("Email already in use.");


        //     using var hmac = new HMACSHA512();

        //     var user = new User
        //     {
        //         Id = Guid.NewGuid(),
        //         Username = dto.Username,
        //         Email = dto.Email,
        //         PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
        //         PasswordSalt = hmac.Key,
        //         CreatedAt = DateTime.UtcNow
        //     };

        //     _context.Users.Add(user);
        //     await _context.SaveChangesAsync();

        //     return Result.Success();
        // }


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
}
