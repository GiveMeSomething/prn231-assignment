using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Auth;
using WebAPI.Base.Guard;
using WebAPI.Base.Jwt;
using WebAPI.Services;

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

