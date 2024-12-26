using System.Security.Claims;

namespace ProEventos.Api.Extensions;

public static class ClainsPrincipalExtension
{
	public static string? GetUserName(this ClaimsPrincipal user) 
		=> user.FindFirst(ClaimTypes.Name)?.Value;

	public static int GetUserId(this ClaimsPrincipal user)
		=> int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
}
