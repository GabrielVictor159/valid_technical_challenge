using Store.Application.Abstractions.DTOs;
using System.Security.Claims;

namespace Store.Api.Extensions;

public static class ClaimsExtensions
{
    public static UserSession GetUserSession(this ClaimsPrincipal user)
    {
        return new UserSession(
            UserId: user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
            Email: user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
            UserName: user.FindFirst("name")?.Value ?? string.Empty
        );
    }
}
