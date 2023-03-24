using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Auth.Decorator;

namespace WebAPI.Auth.Guards
{
    public class AuthGuard : CanActivate
    {
        public override bool canActivate(ActionExecutingContext context)
        {
            return true;
        }
    }
}

