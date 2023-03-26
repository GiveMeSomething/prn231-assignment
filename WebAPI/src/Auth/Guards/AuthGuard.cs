using Microsoft.AspNetCore.Mvc.Filters;
using Utils.DotNest.Guard;
using Utils.Jwt;
using WebAPI.Base.Jwt;

namespace WebAPI.Base.Guard
{
    public class AuthGuard : CanActivate
    {
        public override bool canActivate(ActionExecutingContext context)
        {
            if (context == null || context.ActionDescriptor == null)
            {
                return false;
            }

            var token = context.HttpContext.Request.GetBearerToken(true);
            if (token == null)
            {
                return false;
            }
            return CustomJwt.ValidateToken(token);
        }
    }
}

