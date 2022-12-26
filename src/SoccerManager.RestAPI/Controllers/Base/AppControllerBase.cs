using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SoccerManager.RestAPI.Controllers.Base
{
    public abstract class AppControllerBase : ControllerBase
    {
        protected long? GetAuthenticatedUserId()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return null;
            }
            return long.Parse(userId);
        }
    }
}
