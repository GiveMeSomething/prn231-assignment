using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Auth.Decorator;

namespace WebAPI.Auth.Middleware
{
    public class AuthGuard : CanActivate
    {
        public override bool canActivate(ActionExecutingContext context)
        {
            if(context == null || context.ActionDescriptor == null)
            {
                return false;
            }

            var requiredRoles = (context.ActionDescriptor as ControllerActionDescriptor)
                .MethodInfo
                .GetCustomAttributes<Roles>()
                .FirstOrDefault();

            foreach(string role in requiredRoles.ValidRoles)
            {
                Console.WriteLine(role);
            }

            return true;
        }
    }
}

