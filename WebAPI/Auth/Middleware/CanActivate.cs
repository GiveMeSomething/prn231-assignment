using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Auth.Middleware
{
    public abstract class CanActivate
    {
        public abstract bool canActivate(ActionExecutingContext context);
    }
}

