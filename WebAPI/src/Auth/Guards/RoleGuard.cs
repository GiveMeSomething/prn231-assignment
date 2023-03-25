using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Base.Decorator;

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

            if (requiredRoles == null)
            {
                return true;
            }

            foreach (string role in requiredRoles.ValidRoles)
            {
                // Check roles between JWT payload and endpoints allowed roles
                Console.WriteLine(role);
                return true;
            }

            return false;
        }
    }
}

