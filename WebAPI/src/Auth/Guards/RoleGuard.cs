using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Auth;
using WebAPI.Base.Jwt;
using WebAPI.Services;

namespace WebAPI.Base.Guard
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

            var token = context.HttpContext.Request.GetBearerToken();
            if(token == null)
            {
                return false;
            }

            var userRole = UserFromJwt.Parse(token).Role;

            foreach (var role in requiredRoles.ValidRoles)
            {
                if(userRole == role)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

