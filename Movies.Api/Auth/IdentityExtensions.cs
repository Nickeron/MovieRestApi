namespace Movies.Api.Auth;

public static class IdentityExtensions
{
    public static Guid? GetUserId(this HttpContext context)
    {
        if (
            Guid.TryParse(
                context.User.Claims.SingleOrDefault(claim => claim.Type.Equals("userId"))?.Value,
                out var userId
            )
        )
        {
            return userId;
        }

        return null;
    }
}
