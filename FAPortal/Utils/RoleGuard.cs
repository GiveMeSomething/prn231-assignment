using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using BusinessObject.Models;
using FAPortal.DTOs;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Utils.DotNest.Guard;
using Utils.Jwt;

namespace FAPortal.Utils.Guard
{
    public class RoleGuard : CanActivate
    {
        public override bool canActivate(ActionExecutingContext context)
        {
            if (context == null || context.ActionDescriptor == null)
            {
                return false;
            }

            var requiredRoles = (context.ActionDescriptor as ControllerActionDescriptor)
                .MethodInfo
                .GetCustomAttributes<Roles>()
                .FirstOrDefault();

            if (requiredRoles == null || requiredRoles.ValidRoles.Length == 0)
            {
                return true;
            }

            if(context.HttpContext.Request.Cookies.TryGetValue("jwt", out var token))
            {
                if(token == null)
                {
                    return false;
                }

                if(!CustomJwt.ValidateToken(token))
                {
                    return false;
                }

                var handler = new JwtSecurityTokenHandler();
                var claims = handler.ReadJwtToken(token).Claims;

                var role = claims.FirstOrDefault(claim => claim.Type == "Role")?.Value;
                if (role == null)
                {
                    throw new Exception("JWT missing info. Cannot parse");
                }

                var userRole = (Role)Enum.Parse(typeof(Role), role);
                foreach (var urole in requiredRoles.ValidRoles)
                {
                    if (userRole == urole)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

