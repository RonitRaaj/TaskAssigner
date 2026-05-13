using System.Security.Claims;
public static class ClaimsExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(value, out var id) ? id : 0;
    }
}