using System.Security.Claims;

namespace Retro.Application.Helpers;
    
public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userId, out var guid) ? guid : throw new Exception("Invalid user ID in token.");
    }
}