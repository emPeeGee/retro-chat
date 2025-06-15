using System.ComponentModel.DataAnnotations;

namespace Retro.Application.Models;

public class UserRegisterRequest
{
    [Required] [EmailAddress] public string Email { get; set; } = string.Empty;

    [Required] [MinLength(6)] public string Password { get; set; } = string.Empty;

    [Required] public string Username { get; set; } = string.Empty;
}