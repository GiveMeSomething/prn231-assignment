using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Auth.Guards
{
    public abstract class CanActivate
    {
        public abstract bool canActivate(ActionExecutingContext context);
    }
}

